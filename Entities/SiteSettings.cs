namespace TestAPI.Entities;

public class SiteSettings : BaseEntity
{
    public string SiteName { get; set; } = "PMI Exam Simulator";
    public string SupportEmail { get; set; } = "support@pmi-simulator.com";
    public bool AllowRegistration { get; set; } = true;
    public bool MaintenanceMode { get; set; }
}
