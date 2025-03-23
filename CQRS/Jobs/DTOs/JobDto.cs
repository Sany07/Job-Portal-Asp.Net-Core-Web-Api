using System;

namespace JobPortal.CQRS.Jobs.DTOs
{
    public class JobDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
        public string Location { get; set; }
        public string JobType { get; set; }
        public string Category { get; set; }
        public decimal Salary { get; set; }
        public string CompanyName { get; set; }
        public string CompanyDescription { get; set; }
        public string Url { get; set; }
        public DateTime LastDate { get; set; }
        public bool IsPublished { get; set; }
        public bool IsClosed { get; set; }
        public bool IsActive { get; set; }
        public DateTime Timestamp { get; set; }
        public int UserId { get; set; }
        
        // Additional properties for display
        public string UserFullName { get; set; }
    }
} 