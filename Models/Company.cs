using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JobPortal.Models
{
    public class Company
    {
        public Company()
        {
            Jobs = new HashSet<Job>();
        }

        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        [StringLength(100)]
        public string Industry { get; set; }
        
        [StringLength(100)]
        public string Website { get; set; }
        
        [StringLength(50)]
        public string Location { get; set; }
        
        [StringLength(20)]
        public string PhoneNumber { get; set; }
        
        [StringLength(100)]
        public string Email { get; set; }
        
        [StringLength(255)]
        public string LogoUrl { get; set; }
        
        public DateTime FoundedDate { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        // Navigation property for related jobs
        public virtual ICollection<Job> Jobs { get; set; }
    }
} 