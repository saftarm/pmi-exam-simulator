using TestAPI.Entities;

namespace TestAPI.DTO
{
    public class FinishExamAttemptRequest {

        public Guid ExamAttemptId {get;set;}
        public IEnumerable<UserExamResponseDto>?  userExamResponses {get;set;}


        
    }
}

