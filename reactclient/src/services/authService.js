import api, { ACCESS_TOKEN_KEY, REFRESH_TOKEN_KEY, clearTokens } from './api';

const USER_PROFILE_KEY = 'userProfile';

function parseJwt(token) {
  try {
    const base64 = token.split('.')[1].replace(/-/g, '+').replace(/_/g, '/');
    return JSON.parse(atob(base64));
  } catch {
    return null;
  }
}

function getUserIdFromPayload(payload) {
  if (!payload) return null;
  return (
    payload.nameid ||
    payload.sub ||
    payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'] ||
    null
  );
}

function getUserNameFromPayload(payload) {
  if (!payload) return null;
  return (
    payload.unique_name ||
    payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] ||
    null
  );
}

function getRoleFromPayload(payload) {
  return (
    payload?.role || payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || null
  );
}

function clearProfile() {
  localStorage.removeItem(USER_PROFILE_KEY);
}

function storeProfile(profile) {
  if (profile) {
    localStorage.setItem(USER_PROFILE_KEY, JSON.stringify(profile));
  } else {
    clearProfile();
  }
}

function storeTokens(accessToken, refreshToken) {
  localStorage.setItem(ACCESS_TOKEN_KEY, accessToken);
  localStorage.setItem(REFRESH_TOKEN_KEY, refreshToken);

  const payload = parseJwt(accessToken);
  if (payload) {
    const userId = getUserIdFromPayload(payload);
    const userName = getUserNameFromPayload(payload);
    if (userId) localStorage.setItem('userId', userId);
    if (userName) localStorage.setItem('userName', userName);
  }
}

function userFromJwt(accessToken) {
  const payload = parseJwt(accessToken);
  const userId = getUserIdFromPayload(payload);
  if (!userId) return null;

  return {
    userId,
    id: userId,
    userName: getUserNameFromPayload(payload) || localStorage.getItem('userName'),
    role: getRoleFromPayload(payload),
  };
}

export function getStoredUser() {
  const accessToken = localStorage.getItem(ACCESS_TOKEN_KEY);
  if (!accessToken) return null;

  const tokenUserId = getUserIdFromPayload(parseJwt(accessToken));

  const raw = localStorage.getItem(USER_PROFILE_KEY);
  if (raw) {
    try {
      const profile = JSON.parse(raw);
      const profileId = profile.userId || profile.id;
      if (!tokenUserId || !profileId || String(profileId) === String(tokenUserId)) {
        return profile;
      }
    } catch {
      /* fall through to JWT */
    }
  }

  return userFromJwt(accessToken);
}

export function isAuthenticated() {
  return !!localStorage.getItem(ACCESS_TOKEN_KEY);
}

export async function fetchCurrentUser() {
  const { data } = await api.get('/api/auth/me');
  const profile = {
    userId: data.id,
    id: data.id,
    userName: data.userName,
    displayName: data.displayName,
    email: data.email,
    firstName: data.firstName,
    role: data.role,
    status: data.status,
    createdAt: data.createdAt,
  };
  storeProfile(profile);
  localStorage.setItem('userId', data.id);
  localStorage.setItem('userName', data.userName);
  return profile;
}

export async function login(userName, password) {
  clearProfile();
  const { data } = await api.post('/api/auth/login', { userName, password });
  storeTokens(data.accessToken, data.refreshToken);
  try {
    return await fetchCurrentUser();
  } catch {
    return userFromJwt(data.accessToken);
  }
}

export async function register({ userName, password, firstName, displayName, email }) {
  await api.post('/api/auth/register', { userName, password, firstName, displayName, email });
}

export async function updateProfile(payload) {
  const { data } = await api.patch('/api/auth/me', payload);
  const profile = {
    userId: data.id,
    id: data.id,
    userName: data.userName,
    displayName: data.displayName,
    email: data.email,
    firstName: data.firstName,
    role: data.role,
    status: data.status,
    createdAt: data.createdAt,
  };
  storeProfile(profile);
  return profile;
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
  if (data.errors && Array.isArray(data.errors)) {
    return data.errors.map((e) => e.errorMessage || e.message || String(e)).join(' ');
  }
  if (data.detail) return data.detail;
  if (data.title) return data.title;
  return 'Request failed';
}
