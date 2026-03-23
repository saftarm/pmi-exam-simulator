using System.ComponentModel.DataAnnotations;

namespace TestAPI.Entities
{
    public class AnswerOption
    {

        [Key]
        public int Id { get; set; }
        public int QuestionId { get; set; }

        public Question Question {get;set;} = null!;

        [Required]
        [MaxLength(500)]
        public string Text { get; set; } = string.Empty;

        public bool IsCorrect { get; set; }
        
    }
}
