import { useEffect, useState } from 'react';
import AdminLayout from '../../components/admin/AdminLayout';
import StatusBadge from '../../components/admin/StatusBadge';
import DeleteConfirmModal from '../../components/admin/DeleteConfirmModal';
import Icon from '../../components/Icon';
import { getUsers, saveUsers, logActivity } from '../../services/adminMockStore';

const ROLES = ['Learner', 'Pro', 'Admin'];
const STATUSES = ['Active', 'Suspended', 'Pending'];

export default function AdminUsersPage() {
    const [users, setUsers] = useState([]);
    const [search, setSearch] = useState('');
    const [showForm, setShowForm] = useState(false);
    const [form, setForm] = useState({ name: '', email: '', role: 'Learner', status: 'Active' });
    const [deleteTarget, setDeleteTarget] = useState(null);

    useEffect(() => {
        setUsers(getUsers());
    }, []);

    const persist = (next) => {
        setUsers(next);
        saveUsers(next);
    };

    const filtered = users.filter(
        (u) =>
            u.name.toLowerCase().includes(search.toLowerCase()) ||
            u.email.toLowerCase().includes(search.toLowerCase())
    );

    const handleAdd = (e) => {
        e.preventDefault();
        const initials = form.name
            .split(' ')
            .map((n) => n[0])
            .join('')
            .slice(0, 2)
            .toUpperCase();
        const newUser = { ...form, id: crypto.randomUUID(), initials };
        persist([...users, newUser]);
        logActivity({
            user: 'Admin',
            initials: 'AD',
            action: `Added user "${form.name}"`,
            status: 'Active',
            statusType: 'success',
        });
        setForm({ name: '', email: '', role: 'Learner', status: 'Active' });
        setShowForm(false);
    };

    const handleDelete = () => {
        if (!deleteTarget) return;
        persist(users.filter((u) => u.id !== deleteTarget.id));
        logActivity({
            user: 'Admin',
            initials: 'AD',
            action: `Removed user "${deleteTarget.name}"`,
            status: 'Removed',
            statusType: 'error',
        });
        setDeleteTarget(null);
    };

    const updateUser = (id, field, value) => {
        const next = users.map((u) => (u.id === id ? { ...u, [field]: value } : u));
        persist(next);
    };

    return (
        <AdminLayout title="User Management">
            <div className="mb-lg p-md bg-amber-50 border border-amber-200 rounded-lg text-sm text-amber-800">
                Demo data stored in browser localStorage. Connect a users API when available on the backend.
            </div>

            <div className="flex flex-col sm:flex-row justify-between gap-md mb-lg">
                <div className="relative max-w-md w-full">
                    <Icon
                        name="search"
                        className="absolute left-md top-1/2 -translate-y-1/2 text-on-surface-variant"
                        style={{ fontSize: 20 }}
                    />
                    <input
                        value={search}
                        onChange={(e) => setSearch(e.target.value)}
                        placeholder="Search users…"
                        className="w-full pl-xl pr-md py-sm border border-outline-variant rounded-lg"
                    />
                </div>
                <button
                    type="button"
                    onClick={() => setShowForm(!showForm)}
                    className="bg-secondary-container text-white px-md py-sm rounded-lg font-bold flex items-center gap-sm"
                >
                    <Icon name="person_add" style={{ fontSize: 18 }} />
                    Add User
                </button>
            </div>

            {showForm && (
                <form
                    onSubmit={handleAdd}
                    className="bg-white rounded-xl border border-outline-variant p-lg mb-lg grid grid-cols-1 md:grid-cols-4 gap-md items-end"
                >
                    <input
                        required
                        placeholder="Full name"
                        value={form.name}
                        onChange={(e) => setForm({ ...form, name: e.target.value })}
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
                    <select
                        value={form.role}
                        onChange={(e) => setForm({ ...form, role: e.target.value })}
                        className="border border-outline-variant rounded-lg px-md py-sm"
                    >
                        {ROLES.map((r) => (
                            <option key={r} value={r}>
                                {r}
                            </option>
                        ))}
                    </select>
                    <button type="submit" className="bg-primary text-white py-sm rounded-lg font-bold">
                        Save User
                    </button>
                </form>
            )}

            <div className="bg-white rounded-xl border border-outline-variant shadow-sm overflow-hidden">
                <table className="w-full text-left">
                    <thead className="bg-surface-container-low text-on-surface-variant text-xs uppercase tracking-wider">
                        <tr>
                            <th className="px-lg py-md font-semibold">User</th>
                            <th className="px-lg py-md font-semibold">Email</th>
                            <th className="px-lg py-md font-semibold">Role</th>
                            <th className="px-lg py-md font-semibold">Status</th>
                            <th className="px-lg py-md font-semibold text-right">Actions</th>
                        </tr>
                    </thead>
                    <tbody className="divide-y divide-outline-variant">
                        {filtered.map((user) => (
                            <tr key={user.id} className="hover:bg-surface-container-low/50">
                                <td className="px-lg py-md">
                                    <div className="flex items-center gap-md">
                                        <div className="w-9 h-9 rounded-full bg-primary/10 flex items-center justify-center text-xs font-bold text-primary">
                                            {user.initials}
                                        </div>
                                        <span className="font-medium">{user.name}</span>
                                    </div>
                                </td>
                                <td className="px-lg py-md text-on-surface-variant">{user.email}</td>
                                <td className="px-lg py-md">
                                    <select
                                        value={user.role}
                                        onChange={(e) => updateUser(user.id, 'role', e.target.value)}
                                        className="text-sm border border-outline-variant rounded px-sm py-xs"
                                    >
                                        {ROLES.map((r) => (
                                            <option key={r} value={r}>
                                                {r}
                                            </option>
                                        ))}
                                    </select>
                                </td>
                                <td className="px-lg py-md">
                                    <StatusBadge
                                        status={user.status}
                                        type={user.status === 'Active' ? 'success' : user.status === 'Suspended' ? 'error' : 'warning'}
                                    />
                                </td>
                                <td className="px-lg py-md text-right">
                                    <button
                                        type="button"
                                        onClick={() => setDeleteTarget(user)}
                                        className="text-sm font-bold text-red-600 hover:underline"
                                    >
                                        Remove
                                    </button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>

            <DeleteConfirmModal
                open={!!deleteTarget}
                title="Remove user?"
                message={`Remove "${deleteTarget?.name}" from the demo user list?`}
                onCancel={() => setDeleteTarget(null)}
                onConfirm={handleDelete}
            />
        </AdminLayout>
    );
}
