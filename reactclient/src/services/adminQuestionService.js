import api from './api';

export async function getQuestionsByExam(examId, params = {}) {
    const { data } = await api.get(`/api/exams/${examId}/questions`, { params });
    return data;
}

export async function importQuestions(examId, file) {
    const formData = new FormData();
    formData.append('file', file);
    const { data } = await api.post('/api/questions/import', formData, {
        params: { examId },
        headers: { 'Content-Type': 'multipart/form-data' },
    });
    return data;
}

export async function getQuestion(id) {
    const { data } = await api.get(`/api/questions/${id}`);
    return data;
}

export async function updateQuestion(id, payload) {
    await api.put(`/api/questions/${id}`, { ...payload, id });
}

export async function deleteQuestion(id) {
    await api.delete(`api/questions/${id}`);
}
