using EducationSystem.Application.Repository;
using EducationSystem.Application.ServiceControllers;
using EducationSystem.Application.Validate;
using EducationSystem.Core.Entity.School;
using EducationSystem.Core.Entity.User;
using EducationSystem.Helper.Options;
using EducationSystem.Helper.Request;
using EducationSystem.Helper.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace EducationSystem.BussinesLogic.ServiceController
{
    public class OrderSchoolService : IOrderSchoolService<BaseResponse, RequestOrderSchool, string, string>
    {
        private readonly IBaseCrud<bool, OrderSchool, string> orderSchoolRepository;
        private readonly OptionsBaseAnswer optionsBaseAnswer;
        private readonly IValidate<BaseResponse, OrderSchool> validateOrderSchool;

        public OrderSchoolService(IBaseCrud<bool, OrderSchool, string> orderSchoolRepository,
                                  IOptions<OptionsBaseAnswer> optionsBaseAnswer,
                                  IValidate<BaseResponse, OrderSchool> validateOrderSchool)
        {
            this.orderSchoolRepository = orderSchoolRepository;
            this.optionsBaseAnswer = optionsBaseAnswer.Value;
            this.validateOrderSchool = validateOrderSchool;
        }

        public async Task<BaseResponse> CreateOrderAsync(RequestOrderSchool request)
        {
            var orderSchool = new OrderSchool
            {
                Id_User = request.Id_User,
                Id_School = request.Id_School,
                Message = request.Message
            };

            var resultValidate = await validateOrderSchool.ValidateAsync(orderSchool);

            await orderSchoolRepository.CreateAsync(orderSchool);

            return new BaseResponse(optionsBaseAnswer.Succeeded, 200);
        }

        public async Task<BaseResponse> GetOrderAsync(string id_user)
        {
            var order = await orderSchoolRepository.GetAsync(x => x.Id_User == id_user);
            if (order == null)
                return new BaseResponse(optionsBaseAnswer.NotFound.Replace("{object}", "order school"), 404);

            return new BaseResponse(order, 200);
        }

        public async Task<BaseResponse> GetOrdersPageAsync(int skip, int take)
        {
            var orders = await orderSchoolRepository.GetPageAsync(skip, take);
            if (orders.ToList().Count == 0)
                return new BaseResponse(optionsBaseAnswer.NotFound.Replace("{object}", "orders school"), 404);

            return new BaseResponse(orders, 200);
        }

        public async Task<BaseResponse> UpdateOrderAsync(string id, string request)
        {
            var result = await orderSchoolRepository.UpdateAsync(id, new OrderSchool { Message = request });
            if (result == false)
                return new BaseResponse(optionsBaseAnswer.NotFound.Replace("{object}", "order school"), 404);

            return new BaseResponse(optionsBaseAnswer.Succeeded, 200);
        }

        public async Task<BaseResponse> DeleteOrderAsync(string id)
        {
            var result = await orderSchoolRepository.DeleteAsync(id);
            if(result == false)
                return new BaseResponse(optionsBaseAnswer.NotFound.Replace("{object}", "order school"), 404);

            return new BaseResponse(optionsBaseAnswer.Succeeded, 200);
        }
    }
}
