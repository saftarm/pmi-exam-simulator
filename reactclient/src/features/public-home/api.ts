import api from '../../shared/api/client';
import type { PublicSettingsDto, PublicStatsDto } from './types';

export async function getPublicStats(): Promise<PublicStatsDto> {
  const { data } = await api.get<PublicStatsDto>('/api/public/stats');
  return data;
}

export async function getPublicSettings(): Promise<PublicSettingsDto> {
  const { data } = await api.get<PublicSettingsDto>('/api/public/settings');
  return data;
}
