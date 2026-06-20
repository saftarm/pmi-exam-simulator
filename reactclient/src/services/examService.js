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

export async function getSessionQuestions(sessionId, pageNumber = 1, pageSize = 200) {
    const { data } = await api.get(`/api/session/${sessionId}/questions`, {
        params: { pageNumber, pageSize },
    });
    return data;
}

export async function getSessionQuestionCount(sessionId) {
    const { data } = await api.get('/api/session/questions/count', {
        params: { sessionId },
    });
    return data;
}

export async function finishSession(sessionId, sessionResponses) {
    const { data } = await api.post('/api/session/finish', {
        sessionId,
        sessionResponses,
    });
    return data;
}
