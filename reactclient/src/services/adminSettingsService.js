import api from './api';

export async function getSettings() {
  const { data } = await api.get('/api/admin/settings');
  return data;
}

export async function updateSettings(settings) {
  const { data } = await api.put('/api/admin/settings', settings);
  return data;
}
