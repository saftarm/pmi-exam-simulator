namespace TestAPI.DTO
{
    public class ExamSummaryDto
    {



        public int Id { get; set; }
        public string? Title { get; set; }

        public string CategoryTitle {get; set;} = string.Empty;

        public int CategoryId { get; set; }

        

        public int NumberOfQuestions { get; set; }

        public int DurationInMinutes { get; set; }



    }
}

