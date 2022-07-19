using EducationSystem.Core.Entity.User;
using EducationSystem.Core.Role;
using EducationSystem.Helper.Init;
using EducationSystem.Helper.Options;
using EducationSystem.Helper.Options.OptionsConst;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace EducationSystem.Web.Api.Middlewars
{
    public class DbInitializeMiddleware
    {
        private readonly RequestDelegate next;

        private readonly InitializeDbIdentity initializeDbIdentity;
        private readonly OptionsInitializeAdminAccount optionsInitializeAdminAccount;

        public DbInitializeMiddleware(RequestDelegate next,
                                      InitializeDbIdentity initializeDbIdentity,
                                      IOptions<OptionsInitializeAdminAccount> optionsInitializeAdminAccount)
        {
            this.next = next;

            this.initializeDbIdentity = initializeDbIdentity;
            this.optionsInitializeAdminAccount = optionsInitializeAdminAccount.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await initializeDbIdentity.InitializeRoleAsync(context.RequestServices.GetService<RoleManager<Role>>(),
            new List<Role>
            {
                new Role{Name = OptionsRole.MainAdmin, Level = 1},
                new Role{Name = OptionsRole.Admin, Level = 2},
                new Role{Name = OptionsRole.Director, Level = 3},
                new Role{Name = OptionsRole.Teacher, Level = 4},
                new Role{Name = OptionsRole.Student, Level = 5},
            });

            await initializeDbIdentity.InitializeAdminAsync(context.RequestServices.GetService<UserManager<User>>(),
            new User
            {
                FullName = "Гланвый Администратор",
                UserName = optionsInitializeAdminAccount.UserName,
                Email = optionsInitializeAdminAccount.Email,
                IsAdmin = true
            }, optionsInitializeAdminAccount.Password, OptionsRole.MainAdmin);

            await next.Invoke(context);
        }
    }
}
