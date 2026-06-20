import { useEffect, useState } from 'react';
import AdminLayout from '../../components/admin/AdminLayout';
import DeleteConfirmModal from '../../components/admin/DeleteConfirmModal';
import {
    createCategory,
    deleteCategory,
    getCategories,
    updateCategory,
} from '../../services/adminCategoryService';
import { logActivity } from '../../services/adminMockStore';

export default function AdminCategoriesPage() {
    const [categories, setCategories] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [form, setForm] = useState({ title: '', description: '' });
    const [editing, setEditing] = useState(null);
    const [deleteTarget, setDeleteTarget] = useState(null);
    const [deleting, setDeleting] = useState(false);
    const [saving, setSaving] = useState(false);

    const load = () => {
        setLoading(true);
        getCategories()
            .then(setCategories)
            .catch((err) => setError(err.message || 'Failed to load'))
            .finally(() => setLoading(false));
    };

    useEffect(() => {
        load();
    }, []);

    const handleCreate = async (e) => {
        e.preventDefault();
        setSaving(true);
        setError(null);
        try {
            await createCategory(form);
            logActivity({
                user: 'Admin',
                initials: 'AD',
                action: `Created category "${form.title}"`,
                status: 'Success',
                statusType: 'success',
            });
            setForm({ title: '', description: '' });
            load();
        } catch (err) {
            setError(err.message || 'Create failed');
        } finally {
            setSaving(false);
        }
    };

    const handleUpdate = async (e) => {
        e.preventDefault();
        if (!editing) return;
        setSaving(true);
        try {
            await updateCategory({
                categoryId: editing.id,
                title: editing.title,
                description: editing.description,
            });
            logActivity({
                user: 'Admin',
                initials: 'AD',
                action: `Updated category "${editing.title}"`,
                status: 'Updated',
                statusType: 'info',
            });
            setEditing(null);
            load();
        } catch (err) {
            alert(err.message || 'Update failed');
        } finally {
            setSaving(false);
        }
    };

    const handleDelete = async () => {
        if (!deleteTarget) return;
        setDeleting(true);
        try {
            await deleteCategory(deleteTarget.id);
            logActivity({
                user: 'Admin',
                initials: 'AD',
                action: `Deleted category "${deleteTarget.title}"`,
                status: 'Removed',
                statusType: 'error',
            });
            setDeleteTarget(null);
            load();
        } catch (err) {
            alert(err.message || 'Delete failed');
        } finally {
            setDeleting(false);
        }
    };

    return (
        <AdminLayout title="Categories">
            {error && (
                <div className="mb-lg p-md bg-red-50 text-red-700 rounded-lg border border-red-200">{error}</div>
            )}

            <div className="grid grid-cols-1 lg:grid-cols-3 gap-lg">
                <form
                    onSubmit={handleCreate}
                    className="bg-white rounded-xl border border-outline-variant shadow-sm p-lg space-y-md h-fit"
                >
                    <h2 className="font-headline-sm text-headline-sm font-bold">New Category</h2>
                    <div>
                        <label className="block text-sm font-bold mb-sm">Title</label>
                        <input
                            required
                            value={form.title}
                            onChange={(e) => setForm({ ...form, title: e.target.value })}
                            className="w-full border border-outline-variant rounded-lg px-md py-sm"
                        />
                    </div>
                    <div>
                        <label className="block text-sm font-bold mb-sm">Description</label>
                        <textarea
                            value={form.description}
                            onChange={(e) => setForm({ ...form, description: e.target.value })}
                            rows={3}
                            className="w-full border border-outline-variant rounded-lg px-md py-sm"
                        />
                    </div>
                    <button
                        type="submit"
                        disabled={saving}
                        className="w-full bg-secondary-container text-white py-sm rounded-lg font-bold disabled:opacity-50"
                    >
                        {saving ? 'Saving…' : 'Add Category'}
                    </button>
                </form>

                <div className="lg:col-span-2 bg-white rounded-xl border border-outline-variant shadow-sm overflow-hidden">
                    <div className="p-lg border-b border-outline-variant">
                        <h2 className="font-headline-sm text-headline-sm font-bold">All Categories</h2>
                    </div>
                    {loading ? (
                        <p className="p-lg text-on-surface-variant">Loading…</p>
                    ) : categories.length === 0 ? (
                        <p className="p-lg text-on-surface-variant">No categories yet.</p>
                    ) : (
                        <ul className="divide-y divide-outline-variant">
                            {categories.map((cat) => (
                                <li key={cat.id} className="p-lg">
                                    {editing?.id === cat.id ? (
                                        <form onSubmit={handleUpdate} className="space-y-sm">
                                            <input
                                                required
                                                value={editing.title}
                                                onChange={(e) =>
                                                    setEditing({ ...editing, title: e.target.value })
                                                }
                                                className="w-full border border-outline-variant rounded-lg px-md py-sm"
                                            />
                                            <textarea
                                                required
                                                value={editing.description}
                                                onChange={(e) =>
                                                    setEditing({ ...editing, description: e.target.value })
                                                }
                                                rows={2}
                                                className="w-full border border-outline-variant rounded-lg px-md py-sm"
                                            />
                                            <div className="flex gap-md">
                                                <button
                                                    type="submit"
                                                    className="text-sm font-bold text-secondary-container"
                                                >
                                                    Save
                                                </button>
                                                <button
                                                    type="button"
                                                    onClick={() => setEditing(null)}
                                                    className="text-sm text-on-surface-variant"
                                                >
                                                    Cancel
                                                </button>
                                            </div>
                                        </form>
                                    ) : (
                                        <div className="flex justify-between items-start gap-md">
                                            <div>
                                                <p className="font-bold">{cat.title}</p>
                                                <p className="text-sm text-on-surface-variant mt-xs">
                                                    {cat.description}
                                                </p>
                                            </div>
                                            <div className="flex gap-md shrink-0">
                                                <button
                                                    type="button"
                                                    onClick={() => setEditing({ ...cat })}
                                                    className="text-sm font-bold text-secondary-container hover:underline"
                                                >
                                                    Edit
                                                </button>
                                                <button
                                                    type="button"
                                                    onClick={() => setDeleteTarget(cat)}
                                                    className="text-sm font-bold text-red-600 hover:underline"
                                                >
                                                    Delete
                                                </button>
                                            </div>
                                        </div>
                                    )}
                                </li>
                            ))}
                        </ul>
                    )}
                </div>
            </div>

            <DeleteConfirmModal
                open={!!deleteTarget}
                title="Delete category?"
                message={`Delete "${deleteTarget?.title}"? Exams linked to this category may be affected.`}
                loading={deleting}
                onCancel={() => setDeleteTarget(null)}
                onConfirm={handleDelete}
            />
        </AdminLayout>
    );
}
