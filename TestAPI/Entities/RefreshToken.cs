using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TestAPI.Entities
{
    public class RefreshToken : BaseEntity
    {
        [Required]
        [MaxLength(200)]
        public string TokenHash { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public DateTime ExpiresAt {get;set;}
        public bool Revoked {get;set;} = false;
    }
}