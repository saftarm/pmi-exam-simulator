namespace TestAPI.DTO {

    public class UserProgressSummaryDto {
        public Guid UserId {get;set;}
        public int TotalCertifications {get;set;}
 
        public ICollection<ExamProgressSummaryDto> Exams {get;set;} = new List<ExamProgressSummaryDto>();

    }
}
