using System;
using System.ComponentModel.DataAnnotations;

namespace JobPortal.Models
{
    public class JobSeeker
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }
        
        [StringLength(20)]
        public string PhoneNumber { get; set; }
        
        [StringLength(100)]
        public string CurrentPosition { get; set; }
        
        public string Skills { get; set; }
        
        public string Education { get; set; }
        
        public string Experience { get; set; }
        
        [StringLength(255)]
        public string ResumeUrl { get; set; }
        
        [StringLength(255)]
        public string ProfilePhotoUrl { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime? LastUpdated { get; set; }
    }
} 