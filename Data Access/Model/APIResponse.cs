using System.Net;

namespace InterviewPuzzle.Data_Access.Model
{
    public class APIResponse<T>
    {
        public APIResponse()
        {
            ErrorMessages = new List<string>(); 
        }
        public APIResponse(bool isSuccess, HttpStatusCode statusCode, T result)
        {
            IsSuccess = isSuccess;
            StatusCode = statusCode;
            Result = result;
            ErrorMessages = new List<string>(); 
        }

        public APIResponse(bool isSuccess, HttpStatusCode statusCode, string errorMessages)
        {
            IsSuccess = isSuccess;
            StatusCode = statusCode;
            ErrorMessages = new List<string> { errorMessages };
        }   

        public bool IsSuccess { get; set; }
        public T ?Result { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public List<string> ErrorMessages { get; set; }
    }
}
