using System;
using JobPortal.Models;

namespace JobPortal.CQRS.Users.DTOs
{
    /// <summary>
    /// Data Transfer Object for creating new users during registration
    /// Contains all required information to create a user account
    /// </summary>
    public class CreateUserDto
    {
        /// <summary>
        /// Username for the new account (must be unique)
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Email address for the new account (must be unique)
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Password for the new account (will be hashed before storage)
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Confirmation of the password (must match Password)
        /// </summary>
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// User's first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// User's last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Optional phone number
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Optional date of birth
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// User's gender selection
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// Optional physical address
        /// </summary>
        public string Address { get; set; }
    }
} 