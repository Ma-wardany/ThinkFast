using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineExam.API.Base;
using OnlineExam.Core.Features.Authorization.Commands.Models;
using OnlineExam.Core.Features.Authorization.Queries.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace OnlineExam.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="admin")]
    public class AuthorizationController : AppController
    {

        [HttpPost("add-role")]
        public async Task<IActionResult> AddRole([FromBody]AddRoleCommand command)
        {
            var response = await Mediator.Send(command);
            return FinalResponse(response);
        }

        [HttpPut("update-role")]
        public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleCommand command)
        {
            var response = await Mediator.Send(command);
            return FinalResponse(response);
        }

        [HttpDelete("delete-role")]
        public async Task<IActionResult> DeleteRole([FromQuery] DeleteRoleCommand command)
        {
            var response = await Mediator.Send(command);
            return FinalResponse(response);
        }

        [HttpGet("role-by-id")]
        public async Task<IActionResult> GetRoleById([FromQuery] GetRoleByIdQuery query)
        {
            var response = await Mediator.Send(query);
            return FinalResponse(response);
        }


        [HttpGet("roles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var response = await Mediator.Send(new GetAllRolesQuery());
            return FinalResponse(response);
        }
    }
}
