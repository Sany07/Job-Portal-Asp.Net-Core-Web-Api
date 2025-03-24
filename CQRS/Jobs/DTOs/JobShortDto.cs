using System;
using JobPortal.Enums;

namespace JobPortal.CQRS.Jobs.DTOs
{
    public class JobShortDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string CompanyName { get; set; }
        public string Location { get; set; }
        public JobType JobType { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal Salary { get; set; }
        public DateTime LastDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime Timestamp { get; set; }
    }
} 