namespace JobPortal.CQRS.Categories.DTOs
{
    public class CreateCategoryDto
    {
        public string Name { get; set; }
        public bool IsActive { get; set; } = true;
    }
} 