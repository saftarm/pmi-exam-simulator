using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TestAPI.Enums;

namespace TestAPI.Entities;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

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

    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
    public ICollection<DomainPerformance> DomainPerformances { get; set; } = [];
    public ICollection<ExamAttempt> ExamAttempts { get; set; } = [];

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

    public void UpdateProfile(string displayName, string firstName, string email)
    {
        DisplayName = displayName;
        FirstName = firstName;
        Email = email;
    }
}
