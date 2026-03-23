namespace TestAPI.DTO {

    public class UserProgressSummaryDto {
        public int UserId {get;set;}
        public int TotalCertifications {get;set;}
 
        public ICollection<ExamProgressSummaryDto> Exams {get;set;} = new List<ExamProgressSummaryDto>();

    }
}
