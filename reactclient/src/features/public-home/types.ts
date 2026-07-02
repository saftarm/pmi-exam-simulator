export interface PublicStatsDto {
  totalQuestions: number;
  totalUsers: number;
  publishedExamCount: number;
}

/** Subset of SiteSettingsDto returned by GET /api/public/settings. */
export interface PublicSettingsDto {
  siteName: string;
  allowRegistration: boolean;
  maintenanceMode: boolean;
}
