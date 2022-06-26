
namespace EducationSystem.Helper.Response
{
    public class BaseResponse
    {
        public object Value { get; }
        public int StatusCode { get; }
        public bool Succeded { get; }

        public BaseResponse(object value, int statusCode)
        {
            Value = value;
            StatusCode = statusCode;
        }

        public BaseResponse(object value, int statusCode, bool succeded)
        {
            Value = value;
            StatusCode = statusCode;
            Succeded = succeded;
        }

        public BaseResponse(bool succeded)
        {
            Succeded = succeded;
        }
    }
}
