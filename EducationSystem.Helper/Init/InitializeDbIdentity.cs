using EducationSystem.Core.Entity.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace EducationSystem.Helper.Init
{
    public class InitializeDbIdentity
    {
        private readonly ILogger<InitializeDbIdentity> logger;

        public InitializeDbIdentity(ILogger<InitializeDbIdentity> logger)
        {
            this.logger = logger;
        }

        public async Task InitializeRoleAsync(RoleManager<IdentityRole> roleManager, List<IdentityRole> roles)
        {
            foreach (var role in roles)
            {
                if(await roleManager.FindByNameAsync(role.Name) == null)
                {
                    var result = await roleManager.CreateAsync(role);
                    if (!result.Succeeded)
                        logger.LogWarning(string.Join(" ",result.Errors.Select(x => x.Description)));
                }
            }
        }

        public async Task InitializeAdminAsync(UserManager<User> userManager, User admin, string password, string rolenameAdmin)
        {
            if(await userManager.FindByIdAsync(admin.Id) == null)
            {
                var resultCreateUser = await userManager.CreateAsync(admin, password);
                if (!resultCreateUser.Succeeded)
                    logger.LogWarning(string.Join(" ", resultCreateUser.Errors.Select(x => x.Description)));
                else
                {
                    var resultAddToRole = await userManager.AddToRoleAsync(admin, rolenameAdmin);
                    if(!resultAddToRole.Succeeded)
                        logger.LogWarning(string.Join(" ", resultCreateUser.Errors.Select(x => x.Description)));
                }  
            }
        }

    }
}
