using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace OnlineExam.Core.Bases
{
    public class ResponseHandler
    {

        public Response<T> Success<T>(T? entity, string message = "Successfully!")
        {
            return new Response<T>
            {
                StatusCode = HttpStatusCode.OK,
                Message    = message,
                Data       = entity
            };
        }

        public Response<T> Created<T>(T? entity, string message = "Created!")
        {
            return new Response<T>
            {
                StatusCode = HttpStatusCode.Created,
                Message    = message,
                Data       = entity
            };
        }

        public Response<T> BadRequest<T>(string message = "BadRequest")
        {
            return new Response<T>()
            {
                StatusCode = HttpStatusCode.BadRequest,
                Message    = message
            };
        }

        public Response<T> NotFound<T>(string message = "NotFound")
        {
            return new Response<T>()
            {
                StatusCode = HttpStatusCode.NotFound,
                Message    = message

            };
        }

        public Response<T> UnAuthorized<T>(string message = "UnAuthorized")
        {
            return new Response<T>()
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Message    = message

            };
        }

        public Response<T> UnProcessable<T>(string message = "UnProcessable")
        {
            return new Response<T>()
            {
                StatusCode = HttpStatusCode.UnprocessableEntity,
                Message    = message
            };
        }
    }
}
