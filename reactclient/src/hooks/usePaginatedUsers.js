import { useCallback, useEffect, useState } from 'react';
import { getUsers } from '../services/adminUserService';
import { formatApiErrors } from '../services/authService';

const DEFAULT_PAGE_SIZE = 20;

export function usePaginatedUsers({ search, roleFilter, statusFilter, pageSize = DEFAULT_PAGE_SIZE }) {
  const [users, setUsers] = useState([]);
  const [page, setPage] = useState(1);
  const [totalCount, setTotalCount] = useState(0);
  const [hasNextPage, setHasNextPage] = useState(false);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

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
    } catch (err) {
      setError(formatApiErrors(err));
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
