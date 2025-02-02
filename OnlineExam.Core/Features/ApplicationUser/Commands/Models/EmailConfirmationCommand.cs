using MediatR;
using OnlineExam.Core.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExam.Core.Features.ApplicationUser.Commands.Models
{
    public class EmailConfirmationCommand : IRequest<Response<string>>
    {
        public string userId { get; set; }
        public string code   { get; set; }
    }
}
