using System;
using JobPortal.Enums;

namespace JobPortal.CQRS.Users.DTOs
{
    public class UpdateUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string Address { get; set; }
        // public bool IsActive { get; set; }
    }
} 