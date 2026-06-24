using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestAPI.Entities;

public class SiteSettings
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public string SiteName { get; set; } = "PMI Exam Simulator";
    public string SupportEmail { get; set; } = "support@pmi-simulator.com";
    public bool AllowRegistration { get; set; } = true;
    public bool MaintenanceMode { get; set; }

    public void Update(string siteName, string supportEmail, bool allowRegistration, bool maintenanceMode)
    {
        SiteName = siteName;
        SupportEmail = supportEmail;
        AllowRegistration = allowRegistration;
        MaintenanceMode = maintenanceMode;
    }
}
