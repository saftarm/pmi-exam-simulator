using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace TestAPI.DTO
{
    public class UpdateCategoryDto
    {

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;
    }
}

