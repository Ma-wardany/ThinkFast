using MediatR;
using OnlineExam.Core.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExam.Core.Features.Authentication.Commands.Models
{
    public class ResetPasswordCommand : IRequest<Response<string>>
    {
        public string Email      { get; set; }
        public string ResetToken { get; set; }
        public string NewPassword      { get; set; }
    }
}
