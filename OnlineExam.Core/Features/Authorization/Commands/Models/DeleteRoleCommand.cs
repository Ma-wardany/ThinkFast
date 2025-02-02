using MediatR;
using OnlineExam.Core.Bases;
using System.ComponentModel.DataAnnotations;

namespace OnlineExam.Core.Features.Authorization.Commands.Models
{
    public class DeleteRoleCommand : IRequest<Response<string>>
    {
        [Required(ErrorMessage = "role id is required")]
        public string RoleId { get; set; }
    }
}
