using System;
using System.Threading;
using System.Threading.Tasks;
using JobPortal.CQRS.Auth.DTOs;
using JobPortal.Data;
using JobPortal.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using JobPortal.CQRS.Common.Models;

namespace JobPortal.CQRS.Auth.Queries
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, Result<AuthResponseDto>>
    {
        private readonly JobPortalDbContext _dbContext;
        private readonly ITokenService _tokenService;

        public LoginQueryHandler(JobPortalDbContext dbContext, ITokenService tokenService)
        {
            _dbContext = dbContext;
            _tokenService = tokenService;
        }

        public async Task<Result<AuthResponseDto>> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Find the user by email
                var user = await _dbContext.Users
                    .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

                if (user == null)
                {
                    return Result<AuthResponseDto>.Failure("Invalid email or password");
                }

                // Verify the password
                bool validPassword = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
                if (!validPassword)
                {
                    return Result<AuthResponseDto>.Failure("Invalid email or password");
                }

                // Generate JWT token
                var token = _tokenService.GenerateJwtToken(user);
                var expiresAt = _tokenService.GetTokenExpiration();

                // Create auth response
                var response = new AuthResponseDto
                {
                    UserId = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    Token = token,
                    ExpiresAt = expiresAt
                };

                return Result<AuthResponseDto>.Success(response);
            }
            catch (Exception ex)
            {
                // Log the exception
                return Result<AuthResponseDto>.Failure($"An error occurred during login: {ex.Message}");
            }
        }
    }
} 