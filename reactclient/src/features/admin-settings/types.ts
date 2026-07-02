export interface SiteSettingsDto {
  siteName: string;
  supportEmail: string;
  allowRegistration: boolean;
  maintenanceMode: boolean;
}

export type UpdateSiteSettingsDto = SiteSettingsDto;
