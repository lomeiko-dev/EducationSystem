using EducationSystem.Application.Repository;
using EducationSystem.Application.ServiceControllers;
using EducationSystem.BussinesLogic.ExternalService;
using EducationSystem.BussinesLogic.Repository;
using EducationSystem.BussinesLogic.ServiceController;
using EducationSystem.Core.Entity.Refresh;
using EducationSystem.Core.Entity.User;
using EducationSystem.Helper.Custom;
using EducationSystem.Helper.JWT;
using EducationSystem.Helper.Options;
using EducationSystem.Helper.Request;
using EducationSystem.Helper.Response;
using EducationSystem.Infrastructure.Context;
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
            Builder.Services.AddAuthentication(optionsManager.AuthenticationOption)
                            .AddJwtBearer(optionsManager.JWTOption);
            Builder.Services.AddAuthorization();
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
            Builder.Services.AddTransient<IAuthService<BaseResponse, RequestRegister, RequestLogin, RequestEmailConfirm, string, string>, AuthService>();
        }

        public void AddServicesCrud()
        {
            Builder.Services.AddTransient<IRefreshRepository<bool, RefreshToken, string>, RefreshRepository>();
        }

        public void AddModelOptions()
        {
            Builder.Services.Configure<OptionsEmailApp>(Builder.Configuration.GetSection("EmailApp"))
                            .Configure<OptionsAnswer>(Builder.Configuration.GetSection("Answer"))
                            .Configure<OptionsCustomValidateUser>(Builder.Configuration.GetSection("CustomValidateUser"))
                            .Configure<OptionsJwtValidate>(Builder.Configuration.GetSection("JWTTokenValidate"));
        }

        public void AddServiceHelp()
        {
            Builder.Services.AddTransient<EmailService>()
                            .AddTransient<JwtManager>();

            Builder.Services.AddIdentity<User, IdentityRole>(optionsManager.IdentityUserOption)
                            .AddErrorDescriber<CustomIdentityErrorDescription>()
                            .AddUserValidator<CustomUserValidator>()
                            .AddEntityFrameworkStores<ApplicationContext>()
                            .AddDefaultTokenProviders();

        }

        public void AddServiceHttpContextAccessor()
        {
            Builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            Builder.Services.AddHttpContextAccessor();
        }

        public void AppBuild()
        {
            App = Builder.Build();

            App.UseRouting();
            App.UseAuthentication();
            App.UseAuthorization();
            App.UseCors();

            App.UseEndpoints(point =>
            {
                point.MapControllerRoute(
                    name: "default",
                    pattern: "{Controller}/{Action}");
            });
        }
    }
}
