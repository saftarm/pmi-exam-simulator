import api from './api';

export async function getCategories() {
    const { data } = await api.get('/api/categories');
    return data;
}

export async function getCategory(id) {
    const { data } = await api.get(`/api/categories/${id}`);
    return data;
}

export async function createCategory(payload) {
    await api.post('/api/categories', payload);
}

export async function updateCategory(payload) {
    await api.put(`/api/categories/${payload.categoryId}`, payload);
}

export async function deleteCategory(id) {
    await api.delete(`/api/categories/${id}`);
}
