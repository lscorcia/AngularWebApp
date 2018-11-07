using System.Runtime.Serialization;

namespace AngularWebApp.Infrastructure.Web.ErrorHandling
{
    [DataContract(Name = "ErrorResponse")]
    public class ErrorResponse
    {
        [DataMember(Name = "message")]
        public string Message { get; set; }

        [DataMember(Name = "details")]
        public string Details { get; set; }

        public ErrorResponse(string message): this(message, null)
        {

        }

        public ErrorResponse(string message, string details)
        {
            Message = message;
            Details = details;
        }
    }
}