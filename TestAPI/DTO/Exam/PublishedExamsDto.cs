using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components.Web;

namespace TestAPI.DTO
{
    public class PublishedExamsDto
    {
        public ICollection<PublishedExamDto> PublishedExamDtos = new List<PublishedExamDto>();

    }
}

