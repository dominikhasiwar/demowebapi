using Newtonsoft.Json;
using System.Net;

namespace DemoWebApi.Models
{
    public class ErrorModel
    {
        internal HttpStatusCode StatusCode { get; set; }

        public string ErrorMessage { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

    public class ValidationErrorModel : ErrorModel
    {
        public ValidationError[] Errors{get; set;}
    }

    public class ValidationError
    {
        public string PropertyName { get; set; }

        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }
    }
}
