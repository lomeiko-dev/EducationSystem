using EducationSystem.Application.Repository;
using EducationSystem.Application.Validate;
using EducationSystem.Core.Entity.School;
using EducationSystem.Core.Entity.User;
using EducationSystem.Helper.Options;
using EducationSystem.Helper.Options.OptionsConst;
using EducationSystem.Helper.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace EducationSystem.BussinesLogic.Validate
{
    public class ValidateSchool : IValidate<BaseResponse, School>
    {
        private readonly UserManager<User> userManager;
        private readonly IBaseCrud<bool, School, string> schoolRepository;
        private readonly OptionsBaseAnswer optionsBaseAnswer;
        private readonly OptionsAnswer optionsAnswer;
        private readonly OptionsRole optionsRole;

        public ValidateSchool(UserManager<User> userManager,
                              IBaseCrud<bool, School, string> schoolRepository,
                              IOptions<OptionsAnswer> optionsAnswer,
                              IOptions<OptionsBaseAnswer> optionsBaseAnswer,
                              IOptions<OptionsRole> optionsRole)
        {
            this.userManager = userManager;
            this.schoolRepository = schoolRepository;
            this.optionsAnswer = optionsAnswer.Value;
            this.optionsBaseAnswer = optionsBaseAnswer.Value;
            this.optionsRole = optionsRole.Value;
        }

        public async Task<BaseResponse> ValidateAsync(School obj)
        {
            // get director by request id
            var director = await userManager.FindByIdAsync(obj.Id_Director);
            if (director == null)
                return new BaseResponse(optionsBaseAnswer.NotFound.Replace("{object}", "user"), 404, false);

            var roles = await userManager.GetRolesAsync(director);
            if (roles.FirstOrDefault(x => x == OptionsRole.Director) == null)
                return new BaseResponse(optionsBaseAnswer.NotFound.Replace("{object}", $"role '{OptionsRole.Director}'"), 400);
  
            // check schools
            if (await schoolRepository.GetAsync(x => x.Id_Director == obj.Id_Director) != null)
                return new BaseResponse(optionsAnswer.directorBusy, 400, false);

            return new BaseResponse(true);
        }
    }
}
