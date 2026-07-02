import api, {
  ACCESS_TOKEN_KEY,
  REFRESH_TOKEN_KEY,
  USER_PROFILE_KEY,
  clearTokens,
} from '../../shared/api/client';
import type {
  AuthUser,
  RegisterUserRequest,
  TokenResponse,
  UpdateProfileRequest,
  UserDto,
} from './types';

type JwtPayload = Record<string, unknown>;

function parseJwt(token: string): JwtPayload | null {
  try {
    const base64 = token.split('.')[1].replace(/-/g, '+').replace(/_/g, '/');
    return JSON.parse(atob(base64)) as JwtPayload;
  } catch {
    return null;
  }
}

function getUserIdFromPayload(payload: JwtPayload | null): string | null {
  if (!payload) return null;
  return (
    (payload.nameid as string | undefined) ||
    (payload.sub as string | undefined) ||
    (payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'] as
      | string
      | undefined) ||
    null
  );
}

function getUserNameFromPayload(payload: JwtPayload | null): string | null {
  if (!payload) return null;
  return (
    (payload.unique_name as string | undefined) ||
    (payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] as string | undefined) ||
    null
  );
}

function getRoleFromPayload(payload: JwtPayload | null): string | null {
  if (!payload) return null;
  return (
    (payload.role as string | undefined) ||
    (payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] as string | undefined) ||
    null
  );
}

function clearProfile() {
  localStorage.removeItem(USER_PROFILE_KEY);
}

function storeProfile(profile: AuthUser | null) {
  if (profile) {
    localStorage.setItem(USER_PROFILE_KEY, JSON.stringify(profile));
  } else {
    clearProfile();
  }
}

function storeTokens(accessToken: string, refreshToken: string) {
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

function mapUserDtoToAuthUser(data: UserDto): AuthUser {
  return {
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
}

function userFromJwt(accessToken: string): AuthUser | null {
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

export function getStoredUser(): AuthUser | null {
  const accessToken = localStorage.getItem(ACCESS_TOKEN_KEY);
  if (!accessToken) return null;

  const tokenUserId = getUserIdFromPayload(parseJwt(accessToken));

  const raw = localStorage.getItem(USER_PROFILE_KEY);
  if (raw) {
    try {
      const profile = JSON.parse(raw) as AuthUser;
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

export function isAuthenticated(): boolean {
  return !!localStorage.getItem(ACCESS_TOKEN_KEY);
}

export async function fetchCurrentUser(): Promise<AuthUser> {
  const { data } = await api.get<UserDto>('/api/auth/me');
  const profile = mapUserDtoToAuthUser(data);
  storeProfile(profile);
  localStorage.setItem('userId', data.id);
  localStorage.setItem('userName', data.userName);
  return profile;
}

export async function login(userName: string, password: string): Promise<AuthUser> {
  clearProfile();
  const { data } = await api.post<TokenResponse>('/api/auth/login', { userName, password });
  storeTokens(data.accessToken, data.refreshToken);
  try {
    return await fetchCurrentUser();
  } catch {
    return userFromJwt(data.accessToken)!;
  }
}

export async function register(payload: RegisterUserRequest): Promise<void> {
  await api.post('/api/auth/register', payload);
}

export async function updateProfile(payload: UpdateProfileRequest): Promise<AuthUser> {
  const { data } = await api.patch<UserDto>('/api/auth/me', payload);
  const profile = mapUserDtoToAuthUser(data);
  storeProfile(profile);
  return profile;
}

export function logout() {
  clearTokens();
}
