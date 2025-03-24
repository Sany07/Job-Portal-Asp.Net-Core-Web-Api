using System;
using JobPortal.Enums;

namespace JobPortal.CQRS.Jobs.DTOs
{
    public class CreateJobDto
    {
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
        public DateTime? LastDate { get; set; }
        public bool IsPublished { get; set; } = false;
        public bool IsClosed { get; set; } = false;
        public bool IsActive { get; set; } = true;
    }
} 