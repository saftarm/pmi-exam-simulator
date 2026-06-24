import api from './api';

export async function getAttemptVolume(days = 30) {
  const { data } = await api.get('/api/admin/analytics/attempts', { params: { days } });
  return data;
}

export async function getPassRateAnalytics() {
  const { data } = await api.get('/api/admin/analytics/pass-rate');
  return data;
}
