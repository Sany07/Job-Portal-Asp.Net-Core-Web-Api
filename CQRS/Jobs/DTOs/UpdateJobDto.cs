using System;
using JobPortal.Enums;

namespace JobPortal.CQRS.Jobs.DTOs
{
    public class UpdateJobDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
        public string Location { get; set; }
        public JobType JobType { get; set; }
        public int CategoryId { get; set; }
        public decimal Salary { get; set; }
        public string CompanyName { get; set; }
        public string CompanyDescription { get; set; }
        public string Url { get; set; }
        public DateTime? LastDate { get; set; }
        public bool IsPublished { get; set; }
        public bool IsClosed { get; set; }
        public bool IsActive { get; set; }
    }
} 