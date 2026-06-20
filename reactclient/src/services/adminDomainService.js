import api from './api';

export async function getDomainsByExam(examId) {
    const { data } = await api.get('/api/domains/withTitles', { params: { examId } });
    return data;
}

export async function getDomain(id) {
    const { data } = await api.get(`/api/domains/${id}`);
    return data;
}

export async function updateDomain(id, payload) {
    await api.put(`api/domains/${id}`, payload);
}

export async function deleteDomain(id) {
    await api.delete(`api/domains/${id}`);
}
