using System.Threading.Tasks;
using JobPortal.CQRS.Common;
using JobPortal.CQRS.Users.Commands.CreateUser;
using JobPortal.CQRS.Users.Commands.UpdateUser;
using JobPortal.CQRS.Users.DTOs;
using JobPortal.CQRS.Users.Queries.GetUserById;
using JobPortal.CQRS.Users.Queries.GetUsers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST: api/Users/Register
        [HttpPost("Register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> Register(CreateUserDto createUserDto)
        {
            var command = new CreateUserCommand(createUserDto);
            var result = await _mediator.Send(command);

            if (result.IsFailure)
                return BadRequest(result.Error);

            return CreatedAtAction(nameof(GetUser), new { id = result.Value }, result.Value);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var query = new GetUserByIdQuery(id);
            var result = await _mediator.Send(query);

            if (result.IsFailure)
                return NotFound(result.Error);

            return Ok(result.Value);
        }

        // GET: api/Users
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<UserDto>>> GetUsers([FromQuery] GetUsersQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserDto updateUserDto)
        {
            var command = new UserUpdateCommand(id, updateUserDto);
            var result = await _mediator.Send(command);

            if (result.IsFailure)
            {
                if (result.Error.Contains("not found"))
                    return NotFound(result.Error);
                
                return BadRequest(result.Error);
            }

            return NoContent();
        }
    }
}