using System.Net;

namespace HelloBuild.Domain.Models
{
    public class Response
    {
        public HttpStatusCode StatusCode { get; set; }

        public bool IsSuccess { get; set; }

        public dynamic? Content { get; set; }
    }
}
