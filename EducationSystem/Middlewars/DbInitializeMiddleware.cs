using EducationSystem.Core.Entity.User;
using EducationSystem.Helper.Init;
using EducationSystem.Helper.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace EducationSystem.Web.Api.Middlewars
{
    public class DbInitializeMiddleware
    {
        private readonly RequestDelegate next;

        private readonly InitializeDbIdentity initializeDbIdentity;
        private readonly OptionsInitializeAdminAccount optionsInitializeAdminAccount;
        private readonly OptionsRole optionsRole;

        public DbInitializeMiddleware(RequestDelegate next,
                                      InitializeDbIdentity initializeDbIdentity,
                                      IOptions<OptionsRole> optionsRole,
                                      IOptions<OptionsInitializeAdminAccount> optionsInitializeAdminAccount)
        {
            this.next = next;

            this.initializeDbIdentity = initializeDbIdentity;
            this.optionsInitializeAdminAccount = optionsInitializeAdminAccount.Value;
            this.optionsRole = optionsRole.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await initializeDbIdentity.InitializeRoleAsync(context.RequestServices.GetService<RoleManager<IdentityRole>>(),
            new List<IdentityRole>
            {
                new IdentityRole{Name = optionsRole.MainAdmin},
                new IdentityRole{Name = optionsRole.Admin},
                new IdentityRole{Name = optionsRole.Director},
                new IdentityRole{Name = optionsRole.Teacher},
                new IdentityRole{Name = optionsRole.Student},
            });

            await initializeDbIdentity.InitializeAdminAsync(context.RequestServices.GetService<UserManager<User>>(),
            new User
            {
                FullName = "Гланвый Администратор",
                UserName = optionsInitializeAdminAccount.UserName,
                Email = optionsInitializeAdminAccount.Email,
                IsAdmin = true
            }, optionsInitializeAdminAccount.Password, optionsRole.MainAdmin);

            await next.Invoke(context);
        }
    }
}
