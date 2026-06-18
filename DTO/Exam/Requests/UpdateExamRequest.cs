namespace TestAPI.DTO.Exam.Requests
{
    public record UpdateExamRequest
    {
        public int NumberOfQuestions;
        public int DurationInMinutes;
    }
}
