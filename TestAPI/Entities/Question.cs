using System.ComponentModel.DataAnnotations;
using TestAPI.Models;

namespace TestAPI.Entities
{
    public class Question
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(1000)]
        public string Explanation {get; set;} = string.Empty;

        public int DomainId {get; set;}

        public Domain? Domain {get;set;} = null;

        public int? ExamId {get;set;}

        public Exam? Exam {get;set;} = null ;
        public ICollection<AnswerOption>  AnswerOptions { get; set; } = new List<AnswerOption>();

        public ICollection<UserExamResponse> UserExamResponses { get; set; } = new List<UserExamResponse>();




    }
}
