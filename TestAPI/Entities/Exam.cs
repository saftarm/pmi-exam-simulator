using System.Collections.Generic;

namespace TestAPI.Entities
{
        public class Exam
        {

            public int Id { get; set; }

            public string? Title { get; set; }

            public int PassScore {get;set;}

            public string? Context { get; set; }

            public int? CategoryId {get;set;}

            public Category? Category {get;set;}

            public int DurationInMinutes { get; set; }


            public int NumberOfQuestions { get; set; }

            public ICollection<Domain>? Domains { get; set; }

        

            public ICollection<Question>? Questions {get;set;} = new List<Question>();
            
            public ICollection<AnswerOption>? AnswerOptions {get;set;} 
            
            


        }
}
