namespace TestAPI.DTO.Settings;

public class UpdateSiteSettingsDto
{
    public string SiteName { get; set; } = string.Empty;
    public string SupportEmail { get; set; } = string.Empty;
    public bool AllowRegistration { get; set; }
    public bool MaintenanceMode { get; set; }
    public int DefaultExamDuration { get; set; }
    public int PassThreshold { get; set; }
    public bool NotifyOnNewUser { get; set; }
    public bool NotifyOnExamComplete { get; set; }
}
