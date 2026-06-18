import api, { ACCESS_TOKEN_KEY, REFRESH_TOKEN_KEY, clearTokens } from './api';

function parseJwt(token) {
    try {
        const base64 = token.split('.')[1].replace(/-/g, '+').replace(/_/g, '/');
        return JSON.parse(atob(base64));
    } catch {
        return null;
    }
}

function storeTokens(accessToken, refreshToken) {
    localStorage.setItem(ACCESS_TOKEN_KEY, accessToken);
    localStorage.setItem(REFRESH_TOKEN_KEY, refreshToken);

    const payload = parseJwt(accessToken);
    if (payload) {
        const userId = payload.nameid
            || payload.sub
            || payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
        const userName = payload.unique_name
            || payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'];
        if (userId) localStorage.setItem('userId', userId);
        if (userName) localStorage.setItem('userName', userName);
    }
}

export function getStoredUser() {
    const userId = localStorage.getItem('userId');
    const userName = localStorage.getItem('userName');
    if (!userId || !localStorage.getItem(ACCESS_TOKEN_KEY)) return null;
    return { userId, userName };
}

export function isAuthenticated() {
    return !!localStorage.getItem(ACCESS_TOKEN_KEY);
}

export async function login(userName, password) {
    const { data } = await api.post('/api/auth/login', { userName, password });
    storeTokens(data.accessToken, data.refreshToken);
    return getStoredUser();
}

export async function register({ userName, password, firstName, displayName, email }) {
    await api.post('/api/auth/register', { userName, password, firstName, displayName, email });
}

export function logout() {
    clearTokens();
}

export function formatApiErrors(error) {
    const data = error.response?.data;
    if (!data) return error.message || 'Something went wrong';
    if (Array.isArray(data)) {
        return data.map((e) => e.errorMessage || e.message || String(e)).join(' ');
    }
    if (typeof data === 'string') return data;
    if (data.title) return data.title;
    return 'Request failed';
}
