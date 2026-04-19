using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Talabat.Application.Account.Commands.AddUserToRole;
using Talabat.Application.Account.Commands.ChangePassword;
using Talabat.Application.Account.Commands.DeleteProfilePicture;
using Talabat.Application.Account.Commands.DeleteUser;
using Talabat.Application.Account.Commands.RemoveUserFromRole;
using Talabat.Application.Account.Commands.UpdateUser;
using Talabat.Application.Account.Commands.UploadProfilePicture;
using Talabat.Application.Account.Queries.GetAllUsers;
using Talabat.Application.Account.Queries.GetUser;
using Talabat.Domain.Shared.Constants;

namespace Talabat.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class AccountController(IMediator _mediator) : ControllerBase
    {
        [HttpGet("account")]
        public async Task<IActionResult> GetUser()
        {
            var result = await _mediator.Send(new GetUserQuery());
            return Ok(result);
        }

        [HttpPut("account")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserCommand request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpDelete("account")]
        public async Task<IActionResult> DeleteUser()
        {
            await _mediator.Send(new DeleteUserCommand());
            return NoContent();
        }

        [HttpPut("account/changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand request)
        {
            await _mediator.Send(request);
            return Ok();
        }


        [HttpPost("account/picture")]
        public async Task<IActionResult> AddUserPicture([FromForm] UploadProfilePictureCommand request)
        {
            await _mediator.Send(request);
            return Created();
        }


        [HttpDelete("account/picture")]
        public async Task<IActionResult> DeleteUserPicture()
        {
            await _mediator.Send(new DeleteProfilePictureCommand());
            return NoContent();
        }


        [Authorize(Roles = RoleNames.Admin)]
        [HttpGet("admin/account/users")]
        public async Task<IActionResult> GetAllUsers([FromQuery] GetAllUsersQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }


        [Authorize(Roles = RoleNames.Admin)]
        [HttpPost("admin/account/role")]
        public async Task<IActionResult> AddUserToRole([FromBody] AddUserToRoleCommand request)
        {
            await _mediator.Send(request);
            return Ok();
        }

        [Authorize(Roles = RoleNames.Admin)]
        [HttpDelete("admin/account/role")]
        public async Task<IActionResult> RemoveUserFromRole([FromBody] RemoveUserFromRoleCommand request)
        {
            await _mediator.Send(request);
            return NoContent();
        }
    }
}
