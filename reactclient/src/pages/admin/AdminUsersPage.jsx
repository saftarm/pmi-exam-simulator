import { useCallback, useEffect, useState } from 'react';
import AdminLayout from '../../components/admin/AdminLayout';
import StatusBadge from '../../components/admin/StatusBadge';
import Icon from '../../components/Icon';
import {
    createUser,
    getUsers,
    updateUser,
    updateUserStatus,
} from '../../services/adminUserService';
import { formatApiErrors } from '../../services/authService';
import { formatDate, getInitials } from '../../utils/userDisplay';
import LoadingButton from '../../components/loading/LoadingButton';
import { TableSkeleton, Skeleton } from '../../components/loading';

const ROLES = ['Learner', 'Pro', 'Admin'];
const STATUSES = ['Active', 'Suspended', 'Pending'];

function EditUserModal({ user, saving, onClose, onSave }) {
    const [form, setForm] = useState({
        displayName: user.displayName || '',
        email: user.email || '',
        role: user.role || 'Learner',
        status: user.status || 'Active',
    });

    const handleSubmit = (e) => {
        e.preventDefault();
        onSave(form);
    };

    return (
        <div className="fixed inset-0 z-50 flex items-center justify-center p-md bg-black/40 loading-enter">
            <div className="bg-white rounded-xl border border-outline-variant shadow-lg w-full max-w-md p-lg content-reveal">
                <h2 className="font-headline-sm text-headline-sm font-bold mb-lg">Edit user</h2>
                <form onSubmit={handleSubmit} className="space-y-md">
                    <div>
                        <label className="block text-sm font-bold mb-sm">Display name</label>
                        <input
                            required
                            value={form.displayName}
                            onChange={(e) => setForm({ ...form, displayName: e.target.value })}
                            className="w-full border border-outline-variant rounded-lg px-md py-sm"
                        />
                    </div>
                    <div>
                        <label className="block text-sm font-bold mb-sm">Email</label>
                        <input
                            required
                            type="email"
                            value={form.email}
                            onChange={(e) => setForm({ ...form, email: e.target.value })}
                            className="w-full border border-outline-variant rounded-lg px-md py-sm"
                        />
                    </div>
                    <div>
                        <label className="block text-sm font-bold mb-sm">Role</label>
                        <select
                            value={form.role}
                            onChange={(e) => setForm({ ...form, role: e.target.value })}
                            className="w-full border border-outline-variant rounded-lg px-md py-sm"
                        >
                            {ROLES.map((r) => (
                                <option key={r} value={r}>{r}</option>
                            ))}
                        </select>
                    </div>
                    <div>
                        <label className="block text-sm font-bold mb-sm">Status</label>
                        <select
                            value={form.status}
                            onChange={(e) => setForm({ ...form, status: e.target.value })}
                            className="w-full border border-outline-variant rounded-lg px-md py-sm"
                        >
                            {STATUSES.map((s) => (
                                <option key={s} value={s}>{s}</option>
                            ))}
                        </select>
                    </div>
                    <div className="flex gap-md justify-end pt-md">
                        <button
                            type="button"
                            onClick={onClose}
                            className="px-md py-sm rounded-lg border border-outline-variant font-bold text-sm"
                        >
                            Cancel
                        </button>
                        <LoadingButton
                            type="submit"
                            loading={saving}
                            loadingText="Saving…"
                            className="px-md py-sm rounded-lg bg-secondary-container text-white font-bold text-sm disabled:opacity-50"
                        >
                            Save
                        </LoadingButton>
                    </div>
                </form>
            </div>
        </div>
    );
}

export default function AdminUsersPage() {
    const [users, setUsers] = useState([]);
    const [search, setSearch] = useState('');
    const [roleFilter, setRoleFilter] = useState('');
    const [statusFilter, setStatusFilter] = useState('');
    const [page, setPage] = useState(1);
    const [totalCount, setTotalCount] = useState(0);
    const [hasNextPage, setHasNextPage] = useState(false);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [showForm, setShowForm] = useState(false);
    const [saving, setSaving] = useState(false);
    const [editTarget, setEditTarget] = useState(null);
    const [form, setForm] = useState({
        firstName: '',
        userName: '',
        email: '',
        password: '',
        role: 'Learner',
        status: 'Active',
    });

    const pageSize = 20;

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
    }, [page, search, roleFilter, statusFilter]);

    useEffect(() => {
        const timer = setTimeout(loadUsers, search ? 300 : 0);
        return () => clearTimeout(timer);
    }, [loadUsers, search]);

    useEffect(() => {
        setPage(1);
    }, [roleFilter, statusFilter]);

    const handleAdd = async (e) => {
        e.preventDefault();
        setSaving(true);
        setError(null);
        try {
            await createUser(form);
            setForm({
                firstName: '',
                userName: '',
                email: '',
                password: '',
                role: 'Learner',
                status: 'Active',
            });
            setShowForm(false);
            setPage(1);
            await loadUsers();
        } catch (err) {
            setError(formatApiErrors(err));
        } finally {
            setSaving(false);
        }
    };

    const handleEditSave = async (fields) => {
        if (!editTarget) return;
        setSaving(true);
        try {
            await updateUser(editTarget.id, fields);
            setEditTarget(null);
            await loadUsers();
        } catch (err) {
            alert(formatApiErrors(err));
        } finally {
            setSaving(false);
        }
    };

    const handleQuickStatus = async (user, status) => {
        if (user.status === status) return;
        try {
            await updateUserStatus(user.id, status);
            await loadUsers();
        } catch (err) {
            alert(formatApiErrors(err));
        }
    };

    return (
        <AdminLayout title="User Management">
            {error && (
                <div className="mb-lg p-md bg-red-50 text-red-700 rounded-lg border border-red-200">{error}</div>
            )}

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
                            onChange={(e) => {
                                setSearch(e.target.value);
                                setPage(1);
                            }}
                            placeholder="Search users…"
                            className="w-full pl-xl pr-md py-sm border border-outline-variant rounded-lg"
                        />
                    </div>
                    <select
                        value={roleFilter}
                        onChange={(e) => setRoleFilter(e.target.value)}
                        className="border border-outline-variant rounded-lg px-md py-sm"
                    >
                        <option value="">All roles</option>
                        {ROLES.map((r) => (
                            <option key={r} value={r}>{r}</option>
                        ))}
                    </select>
                    <select
                        value={statusFilter}
                        onChange={(e) => setStatusFilter(e.target.value)}
                        className="border border-outline-variant rounded-lg px-md py-sm"
                    >
                        <option value="">All statuses</option>
                        {STATUSES.map((s) => (
                            <option key={s} value={s}>{s}</option>
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
                <form
                    onSubmit={handleAdd}
                    className="bg-white rounded-xl border border-outline-variant p-lg mb-lg grid grid-cols-1 md:grid-cols-3 lg:grid-cols-6 gap-md items-end"
                >
                    <input
                        required
                        placeholder="First name"
                        value={form.firstName}
                        onChange={(e) => setForm({ ...form, firstName: e.target.value })}
                        className="border border-outline-variant rounded-lg px-md py-sm"
                    />
                    <input
                        required
                        placeholder="Username"
                        value={form.userName}
                        onChange={(e) => setForm({ ...form, userName: e.target.value })}
                        className="border border-outline-variant rounded-lg px-md py-sm"
                    />
                    <input
                        required
                        type="email"
                        placeholder="Email"
                        value={form.email}
                        onChange={(e) => setForm({ ...form, email: e.target.value })}
                        className="border border-outline-variant rounded-lg px-md py-sm"
                    />
                    <input
                        required
                        type="password"
                        placeholder="Password"
                        value={form.password}
                        onChange={(e) => setForm({ ...form, password: e.target.value })}
                        className="border border-outline-variant rounded-lg px-md py-sm"
                    />
                    <select
                        value={form.role}
                        onChange={(e) => setForm({ ...form, role: e.target.value })}
                        className="border border-outline-variant rounded-lg px-md py-sm"
                    >
                        {ROLES.map((r) => (
                            <option key={r} value={r}>{r}</option>
                        ))}
                    </select>
                    <LoadingButton
                        type="submit"
                        loading={saving}
                        loadingText="Saving…"
                        className="bg-primary text-white py-sm rounded-lg font-bold disabled:opacity-50"
                    >
                        Save User
                    </LoadingButton>
                </form>
            )}

            <div className="bg-white rounded-xl border border-outline-variant shadow-sm overflow-x-auto">
                <table className="w-full text-left min-w-[800px]">
                    <thead className="bg-surface-container-low text-on-surface-variant text-xs uppercase tracking-wider">
                        <tr>
                            <th className="px-lg py-md font-semibold">User</th>
                            <th className="px-lg py-md font-semibold">Username</th>
                            <th className="px-lg py-md font-semibold">Email</th>
                            <th className="px-lg py-md font-semibold">Joined</th>
                            <th className="px-lg py-md font-semibold">Role</th>
                            <th className="px-lg py-md font-semibold">Status</th>
                            <th className="px-lg py-md font-semibold text-right">Actions</th>
                        </tr>
                    </thead>
                    <tbody className="divide-y divide-outline-variant">
                        {loading ? (
                            <TableSkeleton rows={6} columns={7} />
                        ) : users.length === 0 ? (
                            <tr>
                                <td colSpan={7} className="px-lg py-xl text-center text-on-surface-variant">
                                    No users found.
                                </td>
                            </tr>
                        ) : (
                            users.map((user) => (
                                <tr key={user.id} className="hover:bg-surface-container-low/50">
                                    <td className="px-lg py-md">
                                        <div className="flex items-center gap-md">
                                            <div className="w-9 h-9 rounded-full bg-primary/10 flex items-center justify-center text-xs font-bold text-primary">
                                                {getInitials(user.displayName)}
                                            </div>
                                            <span className="font-medium">{user.displayName}</span>
                                        </div>
                                    </td>
                                    <td className="px-lg py-md text-on-surface-variant text-sm">{user.userName}</td>
                                    <td className="px-lg py-md text-on-surface-variant">{user.email}</td>
                                    <td className="px-lg py-md text-on-surface-variant text-sm">
                                        {formatDate(user.createdAt)}
                                    </td>
                                    <td className="px-lg py-md text-sm">{user.role}</td>
                                    <td className="px-lg py-md">
                                        <StatusBadge
                                            status={user.status}
                                            type={
                                                user.status === 'Active'
                                                    ? 'success'
                                                    : user.status === 'Suspended'
                                                      ? 'error'
                                                      : 'warning'
                                            }
                                        />
                                    </td>
                                    <td className="px-lg py-md text-right space-x-sm">
                                        <button
                                            type="button"
                                            onClick={() => setEditTarget(user)}
                                            className="text-sm font-bold text-secondary-container hover:underline"
                                        >
                                            Edit
                                        </button>
                                        <select
                                            value={user.status}
                                            onChange={(e) => handleQuickStatus(user, e.target.value)}
                                            className="text-xs border border-outline-variant rounded px-sm py-xs"
                                            aria-label={`Change status for ${user.displayName}`}
                                        >
                                            {STATUSES.map((s) => (
                                                <option key={s} value={s}>{s}</option>
                                            ))}
                                        </select>
                                    </td>
                                </tr>
                            ))
                        )}
                    </tbody>
                </table>
            </div>

            <div className="flex justify-between items-center mt-lg text-sm text-on-surface-variant">
                <span>{totalCount} user{totalCount !== 1 ? 's' : ''} total</span>
                <div className="flex gap-md">
                    <button
                        type="button"
                        disabled={page <= 1 || loading}
                        onClick={() => setPage((p) => p - 1)}
                        className="px-md py-sm border border-outline-variant rounded-lg disabled:opacity-50"
                    >
                        Previous
                    </button>
                    <span className="py-sm">Page {page}</span>
                    <button
                        type="button"
                        disabled={!hasNextPage || loading}
                        onClick={() => setPage((p) => p + 1)}
                        className="px-md py-sm border border-outline-variant rounded-lg disabled:opacity-50"
                    >
                        Next
                    </button>
                </div>
            </div>

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
