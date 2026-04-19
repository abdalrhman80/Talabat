using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Application.Auth.Commands.ConfirmEmail;
using Talabat.Application.Auth.Commands.ForgetPassword;
using Talabat.Application.Auth.Commands.Login;
using Talabat.Application.Auth.Commands.RefreshToken;
using Talabat.Application.Auth.Commands.Register;
using Talabat.Application.Auth.Commands.ResendConfirmation;
using Talabat.Application.Auth.Commands.ResetPassword;
using Talabat.Application.Auth.Commands.RevokeToken;

namespace Talabat.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IMediator _mediator) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(new
            {
                result.Message,
                result.Email,
                result.UserName,
                emailConfirmationRequired = true,
            });
        }

        [HttpGet("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailCommand command)
        {
            await _mediator.Send(command);
            return Ok(new { Message = "Email confirmed successfully. You can now login" });
        }

        [HttpGet("resendConfirmationEmail")]
        public async Task<IActionResult> ResendConfirmation([FromBody] ResendConfirmationCommand command)
        {
            await _mediator.Send(command);
            return Ok(new { Message = "Confirmation email has been resent." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("forgetPassword")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordCommand command)
        {
            await _mediator.Send(command);
            return Ok(new { Message = "Password reset email has been sent." });
        }

        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
        {
            await _mediator.Send(command);
            return Ok(new { Message = "Password has been reset successfully" });
        }

        [HttpGet("refreshToken")]
        public async Task<IActionResult> RefreshToken([FromQuery] RefreshTokenCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("revokeRefreshToken")]
        public async Task<IActionResult> RevokeToken([FromQuery] RevokeTokenCommand command)
        {
            await _mediator.Send(command);
            return Ok(new { Message = "Token is revoked successfully" });
        }
    }
}

