using System;

namespace JobPortal.CQRS.Jobs.DTOs
{
    public class JobShortDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string CompanyName { get; set; }
        public string Location { get; set; }
        public string JobType { get; set; }
        public string Category { get; set; }
        public decimal Salary { get; set; }
        public DateTime LastDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime Timestamp { get; set; }
    }
} 