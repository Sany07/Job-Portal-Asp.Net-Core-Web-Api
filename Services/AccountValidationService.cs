using JobPortal.Data;
using JobPortal.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobPortal.Services
{
    public interface IAccountValidationService
    {
        Task<(bool IsValid, List<string> ErrorMessages)> ValidateUserAsync(User model);
    }

    public class AccountValidationService : IAccountValidationService
    {
        private readonly JobPortalDbContext _context;

        public AccountValidationService(JobPortalDbContext context)
        {
            _context = context;
        }

        public async Task<(bool IsValid, List<string> ErrorMessages)> ValidateUserAsync(User model)
        {
            var errors = new List<string>();
            
            // Skip validation for the current record if it's an update
            if (model.Id != 0)
            {
                // Validate Email uniqueness
                var existingEmail = await _context.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Email == model.Email && u.Id != model.Id);
                
                if (existingEmail != null)
                {
                    errors.Add($"The email '{model.Email}' is already registered. Please use a different email address.");
                }

                // Validate Username uniqueness
                var existingUsername = await _context.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Username == model.Username && u.Id != model.Id);
                
                if (existingUsername != null)
                {
                    errors.Add($"The username '{model.Username}' is already taken. Please choose a different username.");
                }
            }
            else
            {
                // For new records
                // Validate Email uniqueness
                var existingEmail = await _context.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Email == model.Email);
                
                if (existingEmail != null)
                {
                    errors.Add($"The email '{model.Email}' is already registered. Please use a different email address.");
                }

                // Validate Username uniqueness
                var existingUsername = await _context.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Username == model.Username);
                
                if (existingUsername != null)
                {
                    errors.Add($"The username '{model.Username}' is already taken. Please choose a different username.");
                }
            }

            return (errors.Count == 0, errors);
        }
    }
} 