import api from './api';

export async function getUsers(params = {}) {
    const { data } = await api.get('/api/admin/users', { params });
    return data;
}

export async function getUserCount() {
    const { data } = await api.get('/api/admin/users/count');
    return data.count;
}

export async function getUser(id) {
    const { data } = await api.get(`/api/admin/users/${id}`);
    return data;
}

export async function createUser(payload) {
    const { data } = await api.post('/api/admin/users', payload);
    return data;
}

export async function updateUser(id, payload) {
    await api.put(`/api/admin/users/${id}`, payload);
}

export async function updateUserStatus(id, status) {
    await api.patch(`/api/admin/users/${id}/status`, { status });
}

export async function getCurrentUser() {
    const { data } = await api.get('/api/auth/me');
    return data;
}
