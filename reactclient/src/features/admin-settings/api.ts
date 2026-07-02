import api from '../../shared/api/client';
import type { SiteSettingsDto, UpdateSiteSettingsDto } from './types';

export async function getSettings(): Promise<SiteSettingsDto> {
  const { data } = await api.get<SiteSettingsDto>('/api/admin/settings');
  return data;
}

export async function updateSettings(settings: UpdateSiteSettingsDto): Promise<SiteSettingsDto> {
  const { data } = await api.put<SiteSettingsDto>('/api/admin/settings', settings);
  return data;
}
