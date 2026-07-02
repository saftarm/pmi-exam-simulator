import api from '../../shared/api/client';
import type { ApiId, PagedResponse } from '../../shared/api/primitives';
import type {
  CreateUserRequest,
  UpdateUserRequest,
  UpdateUserStatusRequest,
  UserCountDto,
  UserListItemDto,
  UserListQueryParams,
  UserStatsDto,
} from './types';
import type { UserDto } from '../auth/types';

export async function getUsers(params: UserListQueryParams = {}): Promise<PagedResponse<UserListItemDto>> {
  const { data } = await api.get<PagedResponse<UserListItemDto>>('/api/admin/users', { params });
  return data;
}

export async function getUserCount(): Promise<number> {
  const { data } = await api.get<UserCountDto>('/api/admin/users/count');
  return data.count;
}

export async function getUserStats(): Promise<UserStatsDto> {
  const { data } = await api.get<UserStatsDto>('/api/admin/users/stats');
  return data;
}

export async function getUser(id: ApiId): Promise<UserDto> {
  const { data } = await api.get<UserDto>(`/api/admin/users/${id}`);
  return data;
}

export async function updateUserStatus(id: ApiId, status: UpdateUserStatusRequest['status']): Promise<void> {
  const body: UpdateUserStatusRequest = { status };
  await api.patch(`/api/admin/users/${id}/status`, body);
}

/**
 * TODO(backend gap): POST /api/admin/users is not exposed by AdminUserController.
 * Kept for existing admin UI; expect 404 until backend adds the route.
 */
export async function createUser(payload: CreateUserRequest): Promise<UserDto> {
  const { data } = await api.post<UserDto>('/api/admin/users', payload);
  return data;
}

/**
 * TODO(backend gap): PUT /api/admin/users/{id} is not exposed by AdminUserController.
 * Kept for existing admin UI; expect 404 until backend adds the route.
 */
export async function updateUser(id: ApiId, payload: UpdateUserRequest): Promise<void> {
  await api.put(`/api/admin/users/${id}`, payload);
}
