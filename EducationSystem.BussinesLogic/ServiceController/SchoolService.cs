using EducationSystem.Application.Repository;
using EducationSystem.Application.ServiceControllers;
using EducationSystem.Core.Entity.School;
using EducationSystem.Core.Entity.User;
using EducationSystem.Helper.Options;
using EducationSystem.Helper.Request;
using EducationSystem.Helper.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace EducationSystem.BussinesLogic.ServiceController
{
    public class SchoolService : ISchoolService<BaseResponse, RequestSchool, string>
    {
        private readonly IBaseCrud<bool, School, string> schoolRepository;
        private readonly OptionsBaseAnswer optionsBaseAnswer;
        private readonly UserManager<User> userManager;
        private readonly OptionsAnswer optionsAnswer;

        public SchoolService(IBaseCrud<bool, School, string> schoolRepository, 
                             IOptions<OptionsBaseAnswer> optionsBaseAnswer,
                             UserManager<User> userManager,
                             IOptions<OptionsAnswer> optionsAnswer)
        {
            this.schoolRepository = schoolRepository;
            this.optionsBaseAnswer = optionsBaseAnswer.Value;
            this.userManager = userManager;
            this.optionsAnswer = optionsAnswer.Value;
        }

        public async Task<BaseResponse> CreateSchoolAsync(RequestSchool request)
        {
            // get director by request id
            var director = await userManager.FindByIdAsync(request.Id_Director);
            if (director == null)
                return new BaseResponse(optionsBaseAnswer.NotFound.Replace("{object}", "user"), 404);

            // check schools
            if (await schoolRepository.GetAsync(x => x.Id_Director == request.Id_Director) != null)
                return new BaseResponse(optionsAnswer.directorBusy, 400);

            // create and add model
            var school = GetModelByRequestAsync(request);
            await schoolRepository.CreateAsync(school);

            return new BaseResponse(optionsBaseAnswer.Succeeded, 200);
        }

        public async Task<BaseResponse> GetSchoolAsync(string id)
        {
            var school = await schoolRepository.GetAsync(x => x.Id.ToString() == id);
            if(school == null)
                return new BaseResponse(optionsBaseAnswer.NotFound.Replace("{object}", "school"), 404);
            return new BaseResponse(school, 200);
        }

        public async Task<BaseResponse> GetSchoolsPageAsync(int skip, int take)
        {
            var schools = await schoolRepository.GetPageAsync(skip, take);
            if (schools.ToList().Count == 0)
                return new BaseResponse(optionsBaseAnswer.NotFound.Replace("{object}", "schools"), 404);

            return new BaseResponse(schools, 200);
        }

        public async Task<BaseResponse> UpdateSchoolAsync(string id, RequestSchool request)
        {
            // get director by request id
            var director = await userManager.FindByIdAsync(request.Id_Director);
            if (director == null)
                return new BaseResponse(optionsBaseAnswer.NotFound.Replace("{object}", "user"), 404);

            // create and update model
            var school = GetModelByRequestAsync(request);
            var result = await schoolRepository.UpdateAsync(id, school);
            if(!result)
                return new BaseResponse(optionsBaseAnswer.NotFound.Replace("{object}", "user"), 404);

            return new BaseResponse(optionsBaseAnswer.Succeeded, 200);
        }

        public async Task<BaseResponse> DeleteSchoolAsync(string id)
        {
            var result = await schoolRepository.DeleteAsync(id);
            if(!result)
                return new BaseResponse(optionsBaseAnswer.NotFound.Replace("{object}", "school"), 404);
            return new BaseResponse(optionsBaseAnswer.Succeeded, 200);
        }

        #region help methods

        private School GetModelByRequestAsync(RequestSchool request)
        {
            return new School
            {
                Id_Director = request.Id_Director,
                NameSchool = request.NameSchool,
                TypeSchool = request.TypeSchool,
                NumberSchool = request.NumberSchool,
                SchoolAddress = request.SchoolAddress,
                IsClosed = request.Is_Closed,
                Description = request.Description
            };
        }

        #endregion
    }
}
