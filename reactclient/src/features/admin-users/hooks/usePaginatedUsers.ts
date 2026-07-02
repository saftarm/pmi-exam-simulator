import { useCallback, useEffect, useState } from 'react';
import { isAxiosError } from 'axios';
import { getUsers } from '../api';
import { formatApiErrors } from '../../../shared/api/errors';
import type { AccountStatus, UserRole } from '../../auth/types';
import type { UserListItemDto } from '../types';

const DEFAULT_PAGE_SIZE = 20;

interface UsePaginatedUsersOptions {
  search: string;
  roleFilter: UserRole | '';
  statusFilter: AccountStatus | '';
  pageSize?: number;
}

export function usePaginatedUsers({
  search,
  roleFilter,
  statusFilter,
  pageSize = DEFAULT_PAGE_SIZE,
}: UsePaginatedUsersOptions) {
  const [users, setUsers] = useState<UserListItemDto[]>([]);
  const [page, setPage] = useState(1);
  const [totalCount, setTotalCount] = useState(0);
  const [hasNextPage, setHasNextPage] = useState(false);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const loadUsers = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const data = await getUsers({
        pageNumber: page,
        pageSize,
        search: search.trim() || undefined,
        role: roleFilter || undefined,
        status: statusFilter || undefined,
      });
      setUsers(data.items || []);
      setTotalCount(data.totalCount ?? 0);
      setHasNextPage(data.hasNextPage ?? false);
    } catch (err: unknown) {
      const normalizedError = err instanceof Error ? err : new Error(String(err));
      setError(formatApiErrors(isAxiosError(err) ? err : normalizedError));
      setUsers([]);
    } finally {
      setLoading(false);
    }
  }, [page, pageSize, search, roleFilter, statusFilter]);

  useEffect(() => {
    const timer = setTimeout(loadUsers, search ? 300 : 0);
    return () => clearTimeout(timer);
  }, [loadUsers, search]);

  useEffect(() => {
    setPage(1);
  }, [roleFilter, statusFilter]);

  const resetPage = useCallback(() => setPage(1), []);

  return {
    users,
    page,
    setPage,
    totalCount,
    hasNextPage,
    loading,
    error,
    reload: loadUsers,
    resetPage,
  };
}
