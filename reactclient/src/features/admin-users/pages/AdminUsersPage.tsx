import { useState, type ChangeEvent, type FormEvent } from 'react';
import { isAxiosError } from 'axios';
import { AdminLayout } from '../../auth';
import EditUserModal from '../components/EditUserModal';
import { USER_ROLES, USER_STATUSES } from '../constants';
import CreateUserForm from '../components/CreateUserForm';
import UsersTable from '../components/UsersTable';
import Icon from '../../../shared/components/Icon';
import ErrorBanner from '../../../shared/components/ErrorBanner';
import Pagination from '../../../shared/components/Pagination';
import { createUser, updateUser, updateUserStatus } from '../api';
import { formatApiErrors } from '../../../shared/api/errors';
import { usePaginatedUsers } from '../hooks/usePaginatedUsers';
import type { AccountStatus, UserRole } from '../../auth/types';
import type { CreateUserRequest, UpdateUserRequest, UserListItemDto } from '../types';

const EMPTY_CREATE_FORM: CreateUserRequest = {
  firstName: '',
  userName: '',
  email: '',
  password: '',
  role: 'Learner',
  status: 'Active',
};

export default function AdminUsersPage() {
  const [search, setSearch] = useState('');
  const [roleFilter, setRoleFilter] = useState<UserRole | ''>('');
  const [statusFilter, setStatusFilter] = useState<AccountStatus | ''>('');
  const [showForm, setShowForm] = useState(false);
  const [saving, setSaving] = useState(false);
  const [editTarget, setEditTarget] = useState<UserListItemDto | null>(null);
  const [actionError, setActionError] = useState<string | null>(null);
  const [formError, setFormError] = useState<string | null>(null);
  const [form, setForm] = useState(EMPTY_CREATE_FORM);

  const {
    users,
    page,
    setPage,
    totalCount,
    hasNextPage,
    loading,
    error,
    reload,
    resetPage,
  } = usePaginatedUsers({ search, roleFilter, statusFilter });

  const handleAdd = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setSaving(true);
    setFormError(null);
    try {
      await createUser(form);
      setForm(EMPTY_CREATE_FORM);
      setShowForm(false);
      resetPage();
      await reload();
    } catch (err: unknown) {
      const normalizedError = err instanceof Error ? err : new Error(String(err));
      setFormError(formatApiErrors(isAxiosError(err) ? err : normalizedError));
    } finally {
      setSaving(false);
    }
  };

  const handleEditSave = async (fields: UpdateUserRequest) => {
    if (!editTarget) return;
    setSaving(true);
    setActionError(null);
    try {
      await updateUser(editTarget.id, fields);
      setEditTarget(null);
      await reload();
    } catch (err: unknown) {
      const normalizedError = err instanceof Error ? err : new Error(String(err));
      setActionError(formatApiErrors(isAxiosError(err) ? err : normalizedError));
    } finally {
      setSaving(false);
    }
  };

  const handleQuickStatus = async (user: UserListItemDto, status: AccountStatus) => {
    if (user.status === status) return;
    setActionError(null);
    try {
      await updateUserStatus(user.id, status);
      await reload();
    } catch (err: unknown) {
      const normalizedError = err instanceof Error ? err : new Error(String(err));
      setActionError(formatApiErrors(isAxiosError(err) ? err : normalizedError));
    } finally {
      // no-op
    }
  };

  return (
    <AdminLayout title="User Management">
      <ErrorBanner message={actionError} onDismiss={() => setActionError(null)} />
      <ErrorBanner message={formError || error} />

      <div className="flex flex-col lg:flex-row justify-between gap-md mb-lg">
        <div className="flex flex-col sm:flex-row gap-md flex-1">
          <div className="relative flex-1 max-w-md">
            <Icon
              name="search"
              className="absolute left-md top-1/2 -translate-y-1/2 text-on-surface-variant"
              style={{ fontSize: 20 }}
            />
            <input
              value={search}
              onChange={(e: ChangeEvent<HTMLInputElement>) => {
                setSearch(e.target.value);
                resetPage();
              }}
              placeholder="Search users…"
              className="w-full pl-xl pr-md py-sm border border-outline-variant rounded-lg"
            />
          </div>
          <select
            value={roleFilter}
            onChange={(e: ChangeEvent<HTMLSelectElement>) =>
              setRoleFilter(e.target.value as UserRole | '')
            }
            className="border border-outline-variant rounded-lg px-md py-sm"
          >
            <option value="">All roles</option>
            {USER_ROLES.map((r) => (
              <option key={r} value={r}>
                {r}
              </option>
            ))}
          </select>
          <select
            value={statusFilter}
            onChange={(e: ChangeEvent<HTMLSelectElement>) =>
              setStatusFilter(e.target.value as AccountStatus | '')
            }
            className="border border-outline-variant rounded-lg px-md py-sm"
          >
            <option value="">All statuses</option>
            {USER_STATUSES.map((s) => (
              <option key={s} value={s}>
                {s}
              </option>
            ))}
          </select>
        </div>
        <button
          type="button"
          onClick={() => setShowForm(!showForm)}
          className="bg-secondary-container text-white px-md py-sm rounded-lg font-bold flex items-center gap-sm shrink-0"
        >
          <Icon name="person_add" style={{ fontSize: 18 }} />
          Add User
        </button>
      </div>

      {showForm && (
        <CreateUserForm form={form} saving={saving} onChange={setForm} onSubmit={handleAdd} />
      )}

      <UsersTable
        users={users}
        loading={loading}
        onEdit={setEditTarget}
        onStatusChange={handleQuickStatus}
      />

      <Pagination
        page={page}
        hasNextPage={hasNextPage}
        loading={loading}
        onPageChange={setPage}
        totalLabel={`${totalCount} user${totalCount !== 1 ? 's' : ''} total`}
      />

      {editTarget && (
        <EditUserModal
          user={editTarget}
          saving={saving}
          onClose={() => setEditTarget(null)}
          onSave={handleEditSave}
        />
      )}
    </AdminLayout>
  );
}
