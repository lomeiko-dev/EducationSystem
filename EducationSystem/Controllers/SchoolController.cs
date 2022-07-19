using EducationSystem.Application.ServiceControllers;
using EducationSystem.Helper.Extensions;
using EducationSystem.Helper.Request;
using EducationSystem.Helper.Response;
using EducationSystem.Web.Api.Contracts.v1;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EducationSystem.Web.Api.Controllers
{
    [Route(Routes.ControllerSchool)]
    public class SchoolController : Controller
    {
        private readonly ISchoolService<BaseResponse, RequestSchool, string> schoolService;

        public SchoolController(ISchoolService<BaseResponse, RequestSchool, string> schoolService)
        {
            this.schoolService = schoolService;
        }

        [HttpPost(Routes.Create)]
        public async Task<IActionResult> CreateSchoolAsync([FromBody] RequestSchool requestSchool) =>
            await this.Wrap(requestSchool, schoolService.CreateSchoolAsync);

        [HttpGet(Routes.Get)]
        public async Task<IActionResult> GetSchoolAsync([Required] string id) =>
            await this.Wrap(id, schoolService.GetSchoolAsync);

        [HttpGet(Routes.GetPage)]
        public async Task<IActionResult> GetSchoolsAsync([Required] int skip, [Required] int take) =>
            await this.Wrap(skip, take, schoolService.GetSchoolsPageAsync);

        [HttpPut(Routes.Update)]
        public async Task<IActionResult> UpdateSchoolAsync([FromBody] RequestSchool requestSchool, [Required] string id) =>
            await this.Wrap(id, requestSchool, schoolService.UpdateSchoolAsync);

        [HttpDelete(Routes.Delete)]
        public async Task<IActionResult> DeleteSchoolAsync([Required] string id) =>
            await this.Wrap(id, schoolService.DeleteSchoolAsync);
    }
}
