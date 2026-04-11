using System.ComponentModel.DataAnnotations;

namespace TestAPI.Entities
{
    public class DomainPerformance : BaseEntity
    {
        public Guid UserId { get; set; }

        public Guid ExamId { get; set; }

        public Exam? Exam { get; set; }

        public Guid DomainId { get; set; }

        public Domain? Domain { get; set; }


        public int TotalAnswered { get; set; }

        public int TotalCorrect { get; set; }

        public DateTime LastUpdated { get; set; }



    }
}
