using System;

namespace JobPortal.CQRS.Categories.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public int JobCount { get; set; }
    }
} 