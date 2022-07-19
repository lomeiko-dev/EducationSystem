using EducationSystem.Application.Repository;
using EducationSystem.Application.Validate;
using EducationSystem.Core.Entity.School;
using EducationSystem.Core.Entity.User;
using EducationSystem.Helper.Options;
using EducationSystem.Helper.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace EducationSystem.BussinesLogic.Validate
{
    public class ValidateOrderSchool : IValidate<BaseResponse, OrderSchool>
    {
        private readonly UserManager<User> userManager;
        private readonly IBaseCrud<bool, School, string> schoolRepository;
        private readonly IBaseCrud<bool, OrderSchool, string> orderSchoolRepository;
        private readonly OptionsBaseAnswer optionsBaseAnswer;
        private readonly OptionsAnswer optionsAnswer;

        public ValidateOrderSchool(UserManager<User> userManager, 
                                   IBaseCrud<bool, School, string> schoolRepository, 
                                   IBaseCrud<bool, OrderSchool, string> orderSchoolRepository,
                                   IOptions<OptionsBaseAnswer> optionsBaseAnswer,
                                   IOptions<OptionsAnswer> optionsAnswer)
        {
            this.userManager = userManager;
            this.schoolRepository = schoolRepository;
            this.orderSchoolRepository = orderSchoolRepository;
            this.optionsBaseAnswer = optionsBaseAnswer.Value;
            this.optionsAnswer = optionsAnswer.Value;
        }

        public async Task<BaseResponse> ValidateAsync(OrderSchool obj)
        {
            var user = await userManager.FindByIdAsync(obj.Id_User);
            if (user == null)
                return new BaseResponse(optionsBaseAnswer.NotFound.Replace("{object}", "user"), 404, false);

            if (await schoolRepository.GetAsync(x => x.Id.ToString() == user.Id_School) != null)
                return new BaseResponse(optionsAnswer.UserMemberOtherSchool, 400);

            var school = await schoolRepository.GetAsync(x => x.Id.ToString() == obj.Id_School);
            if (school == null)
                return new BaseResponse(optionsBaseAnswer.NotFound.Replace("{object}", "school"), 404, false);

            var order = await orderSchoolRepository.GetAsync(x => x.Id_User == obj.Id_User);
            if (order != null)
                return new BaseResponse(optionsAnswer.userAlreadyOrder, 400, false);

            return new BaseResponse(true);
        }
    }
}
