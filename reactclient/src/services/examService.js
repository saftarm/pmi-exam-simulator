import api from './api';
import { saveSessionQuestions } from '../utils/sessionStorage';

export async function getPublishedExams(pageNumber = 1, pageSize = 50) {
    const { data } = await api.get('/api/exams/details', {
        params: { pageNumber, pageSize },
    });
    return data;
}

export async function getExamDetails(examId) {
    const { data } = await api.get(`/api/exams/${examId}/details`);
    return data;
}

export async function startSession(userId, examId) {
    const { data } = await api.post('/api/session/start', null, {
        params: { userId, examId },
    });
    if (data?.sessionId && data?.questions) {
        saveSessionQuestions(data.sessionId, data.questions);
    }
    return data;
}

export async function finishSession(sessionId, sessionResponses) {
    const { data } = await api.post('/api/session/finish', {
        sessionId,
        sessionResponses,
    });
    return data;
}
