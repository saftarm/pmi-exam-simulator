using TestAPI.Entities;
using TestAPI.Enums;

namespace TestAPI.DTO
{
    public class ExamAttemptDto
    {



        public int Score {get;set;}

        public int CorrectAnswersCount {get;set;}
        public string? ExamTitle {get;set;}

        public ExamStatus Status {get;set;}


    }
}

