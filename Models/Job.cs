using System;
using System.ComponentModel.DataAnnotations;

namespace JobPortal.Models
{
    public class Job
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Company { get; set; }
        
        [StringLength(50)]
        public string Location { get; set; }
        
        [StringLength(50)]
        public string JobType { get; set; } // Full-time, Part-time, Contract, etc.
        
        public decimal? Salary { get; set; }
        
        public DateTime PostedDate { get; set; } = DateTime.Now;
        
        public DateTime? Deadline { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        // Foreign key for the company
        public int CompanyId { get; set; }
        
        // Navigation property
        public virtual Company CompanyNavigation { get; set; }
    }
} 