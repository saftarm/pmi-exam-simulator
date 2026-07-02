import type { ApiId, IsoDateTimeString, PaginationParams } from '../../shared/api/primitives';
import type { AccountStatus, UserRole } from '../auth/types';

export type { AccountStatus, UserRole } from '../auth/types';

export interface UserListItemDto {
  id: ApiId;
  displayName: string;
  email: string;
  role: UserRole;
  status: AccountStatus;
  createdAt: IsoDateTimeString;
}

export interface UserCountDto {
  count: number;
}

export interface UserStatsDto {
  totalCount: number;
  byRole: Record<string, number>;
  byStatus: Record<string, number>;
}

export interface UpdateUserStatusRequest {
  status: AccountStatus;
}

/**
 * TODO(backend gap): CreateUserRequest exists in the service layer but
 * POST /api/admin/users is not exposed by AdminUserController.
 * Kept for existing admin UI; expect 404 until backend adds the route.
 */
export interface CreateUserRequest {
  firstName: string;
  userName: string;
  email: string;
  password: string;
  role?: UserRole;
  status?: AccountStatus;
}

/**
 * TODO(backend gap): UpdateUserRequest exists in the service layer but
 * PUT /api/admin/users/{id} is not exposed by AdminUserController.
 * Kept for existing admin UI; expect 404 until backend adds the route.
 */
export interface UpdateUserRequest {
  displayName: string;
  email: string;
  role: UserRole;
  status: AccountStatus;
}

export interface UserListQueryParams extends PaginationParams {
  search?: string;
  role?: UserRole;
  status?: AccountStatus;
}
