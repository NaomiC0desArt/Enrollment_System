using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UniversitySystem.Application.Exceptions
{
    public class UserFriendlyException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public UserFriendlyException(string message, HttpStatusCode code = HttpStatusCode.BadRequest) : base(message)
        {
            StatusCode = code;
        }
    }
}
