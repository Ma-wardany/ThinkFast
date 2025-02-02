using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OnlineExam.Core.Bases
{
    public class Response<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message            { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T? Data                    { get; set; }

    }
}
