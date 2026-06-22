namespace TestAPI.DTO.Settings;

public record SiteSettingsDto
{
    public string SiteName { get; init; } = string.Empty;
    public string SupportEmail { get; init; } = string.Empty;
    public bool AllowRegistration { get; init; }
    public bool MaintenanceMode { get; init; }
}
