import api from './api';

export async function getDomainPerformances() {
  const { data } = await api.get('/api/progress/domains');
  return data;
}
