import api from './api';

export async function getAllDomains() {
    const { data } = await api.get('/api/domains');
    return Array.isArray(data) ? data : [];
}

export async function getDomainsByExam(examId) {    const { data } = await api.get('/api/domains/withTitles', { params: { examId } });
    const entries = normalizeDomainEntries(data);
    if (entries.length === 0) return [];

    const domains = await Promise.all(
        entries.map(async ([id, title]) => {
            try {
                return await getDomain(id);
            } catch {
                return { id, title, description: '', weight: 1 };
            }
        }),
    );
    return domains;
}

function normalizeDomainEntries(data) {
    if (!data) return [];
    if (Array.isArray(data)) {
        return data.map((d) => [d.id, d.title]);
    }
    return Object.entries(data);
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

// Deferred: POST /api/domains requires examId on CreateDomainDto (backend change).
// Do not call until DomainService associates new domains with an exam.
export async function createDomain(_payload) {
    throw new Error('Adding domains to an existing exam requires a backend update (examId on POST /api/domains).');
}
