using TestAPI.Entities;
using TestAPI.Enums;

namespace TestAPI.DTO
{
    public class UpdatedExamAttemptDto
    {

        public int Score {get;set;}

        public int CorrectAnswersCount {get;set;}

        public ExamStatus Status {get;set;}

        public IEnumerable<UserExamResponse>? UserExamResponses {get;set;}


    }
}

