using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace TestAPI.DTO
{
    public class UpdateCategoryRequest
    {
        public Guid CategoryId {get;set;}
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;
    }
}

