using MediatR;
using OnlineExam.Core.Bases;

namespace OnlineExam.Core.Features.ApplicationUser.Commands.Models
{
    public class ChangePasswordCommand : IRequest<Response<string>>
    {
        public string UserId          { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPssword      { get; set; }
    }
}
