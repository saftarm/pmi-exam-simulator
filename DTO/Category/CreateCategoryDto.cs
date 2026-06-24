using System.ComponentModel.DataAnnotations;

namespace TestAPI.DTO.Category
{
    public class CreateCategoryDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
}

