import api from './api';

export async function getPublicStats() {
  const { data } = await api.get('/api/public/stats');
  return data;
}

export async function getPublicSettings() {
  const { data } = await api.get('/api/public/settings');
  return data;
}
