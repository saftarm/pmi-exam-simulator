import type { ApiId, IsoDateTimeString } from '../../shared/api/primitives';

export type UserRole = 'Learner' | 'Pro' | 'Admin';

export type AccountStatus = 'Active' | 'Suspended' | 'Pending';

export interface LoginUserRequest {
  userName?: string;
  password?: string;
}

export interface RegisterUserRequest {
  userName?: string;
  password?: string;
  firstName?: string;
  displayName?: string;
  email?: string;
}

export interface TokenResponse {
  accessToken: string;
  refreshToken: string;
}

export interface RefreshTokenRequest {
  accessToken?: string;
  refreshToken?: string;
}

export interface RefreshTokenResponse {
  newAccessToken: string;
}

export interface UserDto {
  id: ApiId;
  firstName: string;
  userName: string;
  displayName: string;
  email: string;
  role: UserRole;
  status: AccountStatus;
  createdAt: IsoDateTimeString;
}

export interface UpdateProfileRequest {
  displayName: string;
  firstName: string;
  email: string;
}

/** Persisted auth user shape used by AuthContext (Phase 3). */
export interface AuthUser {
  userId: ApiId;
  id: ApiId;
  userName: string | null;
  displayName?: string;
  email?: string;
  firstName?: string;
  role: UserRole | string | null;
  status?: AccountStatus;
  createdAt?: IsoDateTimeString;
}

/** AuthContext contract (Phase 3). */
export interface AuthContextValue {
  user: AuthUser | null;
  isAuthenticated: boolean;
  isAdmin: boolean;
  profileLoading: boolean;
  authReady: boolean;
  login: (_userName: string, _password: string) => Promise<AuthUser>;
  register: (_payload: RegisterUserRequest) => Promise<void>;
  logout: () => void;
  refreshUser: () => Promise<AuthUser | null>;
}
