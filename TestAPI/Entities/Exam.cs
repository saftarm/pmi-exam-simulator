using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestAPI.Entities
{
        public class Exam
        {
            [Key]
            public int Id { get; set; }

            [Required]
            public string Title { get; set; } = string.Empty;
            public int PassScore {get;set;}

            [Required]
            [MaxLength(500)]
            public string Context { get; set; } = string.Empty;
            public ExamStatus Status {get;set;}

            public int CategoryId {get;set;}

            public Category Category {get;set;} = null!;

            [Range(0, 360)]
            public int DurationInMinutes { get; set; }
            public int NumberOfQuestions { get; set; }
 
            public ICollection<Domain> Domains { get; set; } = new List<Domain>();

            public ICollection<DomainPerformance> DomainPerfomances {get;set;} = new List<DomainPerformance>();
        
            public ICollection<Question> Questions {get;set;} = new List<Question>();

        }
}
