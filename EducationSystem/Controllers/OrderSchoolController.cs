using EducationSystem.Application.ServiceControllers;
using EducationSystem.Helper.Extensions;
using EducationSystem.Helper.Request;
using EducationSystem.Helper.Response;
using EducationSystem.Web.Api.Contracts.v1;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EducationSystem.Web.Api.Controllers
{
    [Route(Routes.ControllerOrderSchool)]
    public class OrderSchoolController : Controller
    {
        private readonly IOrderSchoolService<BaseResponse, RequestOrderSchool, string, string> schoolService;

        public OrderSchoolController(IOrderSchoolService<BaseResponse, RequestOrderSchool, string, string> schoolService)
        {
            this.schoolService = schoolService;
        }

        [HttpPost(Routes.Create)]
        public async Task<IActionResult> CreateOrderSchoolAsync([FromBody] RequestOrderSchool request) =>
            await this.Wrap(request, schoolService.CreateOrderAsync);

        [HttpGet(Routes.Get)]
        public async Task<IActionResult> GetOrderSchoolAsync([Required] string id) =>
            await this.Wrap(id, schoolService.GetOrderAsync);

        [HttpGet(Routes.GetPage)]
        public async Task<IActionResult> GetOrderSchoolsAsync([Required]int skip, [Required]int take) =>
            await this.Wrap(skip, take, schoolService.GetOrdersPageAsync);

        [HttpPut(Routes.Update)]
        public async Task<IActionResult> UpdateOrderSchoolAsync([Required]string id, [FromBody] string request) =>
            await this.Wrap(id, request, schoolService.UpdateOrderAsync);

        [HttpDelete(Routes.Delete)]
        public async Task<IActionResult> DeleteOrderSchoolAsync([Required] string id) =>
            await this.Wrap(id, schoolService.DeleteOrderAsync);
    }
}
