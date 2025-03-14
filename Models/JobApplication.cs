using System;
using System.ComponentModel.DataAnnotations;

namespace JobPortal.Models
{
    public class JobApplication
    {
        [Key]
        public int Id { get; set; }
        
        public int JobId { get; set; }
        
        public int JobSeekerId { get; set; }
        
        [StringLength(255)]
        public string CoverLetter { get; set; }
        
        [StringLength(255)]
        public string ResumeUrl { get; set; }
        
        public DateTime ApplicationDate { get; set; } = DateTime.Now;
        
        [StringLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, Reviewed, Shortlisted, Rejected, etc.
        
        public string Notes { get; set; }
        
        // Navigation properties
        public virtual Job Job { get; set; }
        
        public virtual JobSeeker JobSeeker { get; set; }
    }
} 