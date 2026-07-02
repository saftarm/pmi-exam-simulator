import api from '../../shared/api/client';
import type { AttemptVolumeDto } from './types';

export async function getAttemptVolume(days = 30): Promise<AttemptVolumeDto[]> {
  const { data } = await api.get<AttemptVolumeDto[]>('/api/admin/analytics/attempts', {
    params: { days },
  });
  return data;
}

export async function getPassRateAnalytics() {
  const { data } = await api.get('/api/admin/analytics/pass-rate');
  return data;
}
