using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using JobPortal.Enums;

namespace JobPortal.Models
{
    public class Job
    {
        [Key]
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public string Tags { get; set; }
        
        public string Location { get; set; }
        
        public JobType JobType { get; set; }
        
        public string Category { get; set; }
        
        public decimal Salary { get; set; }
        
        public string CompanyName { get; set; }
        
        public string CompanyDescription { get; set; }
        
        public string Url { get; set; }
        
        public DateTime LastDate { get; set; } = DateTime.UtcNow.AddDays(30);
        
        public bool IsPublished { get; set; } = false;
        
        public bool IsClosed { get; set; } = false;
        
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        
        // Foreign key relationship to User (as the creator/employer)
        public int UserId { get; set; }
        
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
} 