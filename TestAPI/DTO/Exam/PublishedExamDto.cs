using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components.Web;

namespace TestAPI.DTO
{
    public class PublishedExamDto
    {
        public int Id {get;set;}
        public string Title {get;set;} = string.Empty;
        public string Category {get; set;} = string.Empty;

        public int DurationInMinutes {get; set;}

        public int NumberOfQuestions  {get; set;}


    }
}

