using MediatR;
using OnlineExam.Core.Bases;
using OnlineExam.Domain.Entities.Identity;
using System.ComponentModel.DataAnnotations;

namespace OnlineExam.Core.Features.Authorization.Commands.Models
{
    public class UpdateRoleCommand : IRequest<Response<Role>>
    {
        [Required(ErrorMessage = "role id is required")]
        public string RoleId { get; set; }

        [Required(ErrorMessage = "role name is required")]
        public string RoleName { get; set; }
    }
}
