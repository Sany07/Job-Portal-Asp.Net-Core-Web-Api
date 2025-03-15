using System;
using JobPortal.Models;

namespace JobPortal.CQRS.Users.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; }
        
        // Note: Password is excluded from the DTO for security
    }
} 