import api from './api';

export async function getAllExams() {
    const { data } = await api.get('/api/exams');
    return data;
}

export async function getExamDetails(examId) {
    const { data } = await api.get(`/api/exams/${examId}/details`);
    return data;
}

export async function createExam(payload) {
    await api.post('/api/exams/', payload);
}

export async function updateExam(examId, payload) {
    await api.patch(`/api/exams/${examId}/update`, payload);
}

export async function publishExam(examId) {
    await api.post(`/api/exams/${examId}/publish`);
}

export async function deleteExam(examId) {
    await api.delete(`/api/exams/${examId}`);
}

export async function deleteExamsBulk(examIds) {
    await api.delete('/api/exams', { params: { examIds } });
}

export async function getExamOverviewStats() {
    const { data } = await api.get('/api/admin/exams/stats');
    return data;
}
