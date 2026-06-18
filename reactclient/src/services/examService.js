import api from './api';

export async function getPublishedExams(pageNumber = 1, pageSize = 50) {
    const { data } = await api.get('/api/exams/details', {
        params: { pageNumber, pageSize },
    });
    return data;
}

export async function startSession(userId, examId) {
    const { data } = await api.post('/api/session/start', null, {
        params: { userId, examId },
    });
    return data;
}
