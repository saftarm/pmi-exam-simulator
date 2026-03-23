using System.ComponentModel.DataAnnotations;

namespace TestAPI.Entities
{
    public class User
    {

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string FirstName {get; set;} = string.Empty;

        [Required]
        [MaxLength(50)]
        public string UserName {get; set;} = string.Empty;

        [Required]
        [MaxLength(50)]
        public string DisplayName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Email {get;set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string PasswordHash {get;set;} = string.Empty;

        public ICollection<DomainPerformance>  DomainPerfomances {get;set;} = new List<DomainPerformance>();

        public ICollection<ExamAttempt> ExamAttempts {get;set;} = new List<ExamAttempt>();



        


    }
}
