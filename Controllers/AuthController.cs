using System.Threading.Tasks;
using JobPortal.CQRS.Auth.DTOs;
using JobPortal.CQRS.Auth.Queries;
using JobPortal.CQRS.Users.Commands.CreateUser;
using JobPortal.CQRS.Users.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] CreateUserDto userDto)
        {
            // Create user command using existing CQRS command
            var command = new CreateUserCommand(userDto);
            var result = await _mediator.Send(command);
            
            if (result.IsSuccess)
            {
                // If registration succeeds, auto-login the user
                var loginDto = new LoginDto
                {
                    Email = userDto.Email,
                    Password = userDto.Password
                };
                
                var loginQuery = new LoginQuery(loginDto);
                var loginResult = await _mediator.Send(loginQuery);
                
                if (loginResult.IsSuccess)
                {
                    // Return auth response with token
                    return CreatedAtAction(nameof(Login), null, loginResult.Value);
                }
                
                // If auto-login fails, return success without token
                return StatusCode(StatusCodes.Status201Created, 
                    new { UserId = result.Value, Message = "Registration successful. Please log in." });
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var query = new LoginQuery(loginDto);
            var result = await _mediator.Send(query);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return Unauthorized(new { Message = result.Error });
        }
    }
} 
