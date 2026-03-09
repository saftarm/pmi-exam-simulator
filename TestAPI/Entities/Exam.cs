namespace TestAPI.Entities
{
        public class Exam
        {

            public int Id { get; set; }

            public string? Title { get; set; }

            public int NumberOfQuestions { get; set; }

            public ICollection<Question>? Questions {get;set;}
            public ICollection<ExamQuestion>? ExamsQuestions { get; set; }
            public int DurationInMinutes { get; set; }





        }
}
