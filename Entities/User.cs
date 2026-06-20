using System.ComponentModel.DataAnnotations;
using TestAPI.Enums;

namespace TestAPI.Entities
{
    public class User : BaseEntity
    {

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string DisplayName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        public UserRole Role { get; set; } = UserRole.Learner;

        public AccountStatus Status { get; set; } = AccountStatus.Active;

        [Required]
        public ICollection<RefreshToken> RefreshTokens {get;set;} = new List<RefreshToken>();
        public ICollection<DomainPerformance> DomainPerfomances { get; set; } = new List<DomainPerformance>();
        public ICollection<ExamAttempt> ExamAttempts { get; set; } = new List<ExamAttempt>();

        public void UpdateAdminProfile(string displayName, string email, UserRole role, AccountStatus status)
        {
            DisplayName = displayName;
            Email = email;
            Role = role;
            Status = status;
        }

        public void SetStatus(AccountStatus status)
        {
            Status = status;
        }
    }
}
