import api from './api';

export async function getQuestions(params = {}) {
    const { data } = await api.get('/api/questions', { params });
    return data;
}

export async function importQuestions(file) {
    const formData = new FormData();
    formData.append('file', file);
    const { data } = await api.post('/api/questions/import', formData, {
        headers: { 'Content-Type': 'multipart/form-data' },
    });
    return data;
}

export async function getQuestion(id) {
    const { data } = await api.get(`/api/questions/${id}`);
    return data;
}

export async function createQuestion(payload) {
    await api.post('/api/questions', payload);
}

export async function updateQuestion(id, payload) {
    await api.put(`/api/questions/${id}`, { ...payload, id });
}

export async function deleteQuestion(id) {
    await api.delete(`/api/questions/${id}`);
}

export async function bulkDeleteQuestions(questionIds) {
    await api.delete('/api/questions', { data: { questionIds } });
}
