using JobPortal.CQRS.Auth.DTOs;
using JobPortal.CQRS.Common.Models;
using MediatR;

namespace JobPortal.CQRS.Auth.Queries
{
    /// <summary>
    /// Query for authenticating a user and generating a JWT token
    /// </summary>
    public class LoginQuery : IRequest<Result<AuthResponseDto>>
    {
        public string Email { get; }
        public string Password { get; }

        public LoginQuery(LoginDto loginDto)
        {
            Email = loginDto.Email;
            Password = loginDto.Password;
        }
    }
} 