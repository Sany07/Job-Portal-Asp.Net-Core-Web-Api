using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using JobPortal.Enums;
using System.Collections.Generic;

namespace JobPortal.Models
{
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(Username), IsUnique = true)]
    public class User
    {
        [Key]
        public int Id { get; set; }
        
        public string Username { get; set; }
        
        public string Email { get; set; }
        
        public string Password { get; set; }

        [NotMapped] 
        public string ConfirmPassword { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public string PhoneNumber { get; set; }
        
        public DateTime? DateOfBirth { get; set; }
        
        public Gender Gender { get; set; }
        
        public string Address { get; set; }

        public UserRole Role { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public bool IsActive { get; set; } = true;

        // Navigation property for jobs created by this user
        public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();

        // Custom validation method for age verification (if needed)
        // public bool IsValidAge()
        // {
        //     if (DateOfBirth.HasValue)
        //     {
        //         var age = DateTime.Today.Year - DateOfBirth.Value.Year;
        //         if (DateOfBirth.Value.Date > DateTime.Today.AddYears(-age))
        //             age--;
                
        //         return age >= 18; // Example: ensuring the user is at least 18 years old
        //     }
        //     return true; // Not required if no DOB provided
        // }
    }
} 