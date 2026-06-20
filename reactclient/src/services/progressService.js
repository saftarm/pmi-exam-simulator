import api from './api';

export async function getDomainPerformances(userId) {
    const { data } = await api.get('/api/progress/domains', { params: { userId } });
    return data;
}
