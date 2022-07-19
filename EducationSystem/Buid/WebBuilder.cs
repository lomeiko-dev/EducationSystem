using EducationSystem.Application.Repository;
using EducationSystem.Application.ServiceControllers;
using EducationSystem.Application.Validate;
using EducationSystem.BussinesLogic.ExternalService;
using EducationSystem.BussinesLogic.Repository;
using EducationSystem.BussinesLogic.ServiceController;
using EducationSystem.BussinesLogic.Validate;
using EducationSystem.Core.Entity.School;
using EducationSystem.Core.Entity.User;
using EducationSystem.Core.Role;
using EducationSystem.Helper.Custom;
using EducationSystem.Helper.Init;
using EducationSystem.Helper.JWT;
using EducationSystem.Helper.Options;
using EducationSystem.Helper.Options.OptionsConst;
using EducationSystem.Helper.Request;
using EducationSystem.Helper.Response;
using EducationSystem.Infrastructure.Context;
using EducationSystem.Web.Api.Middlewars;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace EducationSystem.Web.Api.Buid
{
    internal class WebBuilder
    {
        public WebApplicationBuilder Builder { get; }
        public WebApplication App { get; private set; }

        private OptionsManager optionsManager;

        public WebBuilder(string[] args)
        {
            Builder = WebApplication.CreateBuilder(args);
            optionsManager = new OptionsManager(Builder);
        }

        public void AddDb()
        {
            Builder.Services.AddDbContext<ApplicationContext>(optionsManager.DbContextOption);
        }

        public void AddAuthorizeAuthentication()
        {
            Builder.Services.AddAuthorization();
            Builder.Services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                            .AddJwtBearer(optionsManager.JWTOption); // JWT
        }

        public void AddCors()
        {
            Builder.Services.AddCors(optionsManager.CorsOption);
        }

        public void AddControllers()
        {
            Builder.Services.AddControllers();
        }

        public void AddServicesControllers()
        {
            Builder.Services.AddTransient<IAuthService<BaseResponse, RequestRegister, RequestLogin, RequestEmailConfirm, string, string>, AuthService>()
                            .AddTransient<ISchoolService<BaseResponse, RequestSchool, string>, SchoolService>()
                            .AddTransient<IOrderSchoolService<BaseResponse, RequestOrderSchool, string, string>, OrderSchoolService>()
                            .AddTransient<IUserService<BaseResponse, User, string>, UserService>();
        }

        public void AddServicesCrud()
        {
            Builder.Services.AddTransient<IBaseCrud<bool, School, string>, SchoolRepository>()
                            .AddTransient<IBaseCrud<bool, OrderSchool, string>, OrderSchoolRepository>();
        }

        public void AddModelOptions()
        {
            Builder.Services.Configure<OptionsEmailApp>(Builder.Configuration.GetSection("EmailApp"))
                            .Configure<OptionsAnswer>(Builder.Configuration.GetSection("Answer"))
                            .Configure<OptionsCustomValidateUser>(Builder.Configuration.GetSection("CustomValidateUser"))
                            .Configure<OptionsJwtValidate>(Builder.Configuration.GetSection("JWTTokenValidate"))
                            .Configure<OptionsBaseAnswer>(Builder.Configuration.GetSection("BaseAnswer"))
                            .Configure<OptionsRole>(Builder.Configuration.GetSection("Role"))
                            .Configure<OptionsInitializeAdminAccount>(Builder.Configuration.GetSection("InitializeAdminAccount"));
        }

        public void AddServiceHelp()
        {
            Builder.Services.AddTransient<EmailService>()
                            .AddTransient<JwtManager>();

            Builder.Services.AddIdentity<User, Role>(optionsManager.IdentityUserOption)
                            .AddErrorDescriber<CustomIdentityErrorDescription>()
                            .AddUserValidator<CustomUserValidator>()
                            .AddEntityFrameworkStores<ApplicationContext>()
                            .AddDefaultTokenProviders();

            Builder.Services.AddTransient<InitializeDbIdentity>();

            Builder.Services.AddTransient<IValidate<BaseResponse, School>, ValidateSchool>()
                            .AddTransient<IValidate<BaseResponse, OrderSchool>, ValidateOrderSchool>()
                            .AddTransient<IValidate<IEnumerable<string>, RequestValidateAddedRole>, ValidatePermissionAddedRole>();

        }

        public void AddServiceHttpContextAccessor()
        {
            Builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            Builder.Services.AddHttpContextAccessor();
        }

        public void AppBuild()
        {
            App = Builder.Build();

            App.UseMiddleware<DbInitializeMiddleware>();

            App.UseCors();

            App.UseRouting();

            App.UseAuthentication();
            App.UseAuthorization();

            App.UseEndpoints(point =>
            {
                point.MapControllerRoute(
                    name: "default",
                    pattern: "{Controller}/{Action}");
            });

            App.Run();
        }
    }
}
