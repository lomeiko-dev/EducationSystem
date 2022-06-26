using EducationSystem.Helper.Response;
using Microsoft.AspNetCore.Mvc;

namespace EducationSystem.Helper.Extensions
{
    public static class ControllerExtensions
    {
        public static async Task<IActionResult> Take<TItem>(this ControllerBase controller, TItem item, Func<TItem, Task<BaseResponse>> func)
        {
            if(controller.ModelState.IsValid)
            {
                var result = await func.Invoke(item);
                return controller.StatusCode(result.StatusCode, result.Value);
            }
            return controller.StatusCode(400, "Данные не валидны");
        }

        public static async Task<IActionResult> Take<TItem, TItem2>(this ControllerBase controller, TItem item, TItem2 item2, Func<TItem, TItem2, Task<BaseResponse>> func)
        {
            if (controller.ModelState.IsValid)
            {
                var result = await func.Invoke(item, item2);
                return controller.StatusCode(result.StatusCode, result.Value);
            }
            return controller.StatusCode(400, "Данные не валидны");
        }

        public static async Task<IActionResult> Take<TItem, TItem2, TItem3>(this ControllerBase controller, TItem item, TItem2 item2, TItem3 item3, Func<TItem, TItem2, TItem3, Task<BaseResponse>> func)
        {
            if (controller.ModelState.IsValid)
            {
                var result = await func.Invoke(item, item2, item3);
                return controller.StatusCode(result.StatusCode, result.Value);
            }
            return controller.StatusCode(400, "Данные не валидны");
        }
    }
}
