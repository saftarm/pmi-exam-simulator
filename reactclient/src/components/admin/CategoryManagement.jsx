import { useEffect, useState } from 'react';
import DeleteConfirmModal from './DeleteConfirmModal';
import {
  createCategory,
  deleteCategory,
  getCategories,
  updateCategory,
} from '../../services/adminCategoryService';
import { formatApiErrors } from '../../services/authService';
import { Skeleton, LoadingButton } from '../loading';

export default function CategoryManagement() {
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
      setForm({ title: '', description: '' });
      load();
    } catch (err) {
      setError(formatApiErrors(err));
    } finally {
      setSaving(false);
    }
  };

  const handleUpdate = async (e) => {
    e.preventDefault();
    if (!editing) return;
    setSaving(true);
    setError(null);
    try {
      await updateCategory({
        categoryId: editing.id,
        title: editing.title,
        description: editing.description,
      });
      setEditing(null);
      load();
    } catch (err) {
      setError(formatApiErrors(err));
    } finally {
      setSaving(false);
    }
  };

  const handleDelete = async () => {
    if (!deleteTarget) return;
    setDeleting(true);
    setError(null);
    try {
      await deleteCategory(deleteTarget.id);
      setDeleteTarget(null);
      load();
    } catch (err) {
      setError(formatApiErrors(err));
    } finally {
      setDeleting(false);
    }
  };

  return (
    <>
      {error && (
        <div className="mb-lg p-md bg-red-50 text-red-700 rounded-lg border border-red-200">
          {error}
        </div>
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
              maxLength={200}
              value={form.title}
              onChange={(e) => setForm({ ...form, title: e.target.value })}
              className="w-full border border-outline-variant rounded-lg px-md py-sm"
            />
          </div>
          <div>
            <label className="block text-sm font-bold mb-sm">Description</label>
            <textarea
              maxLength={500}
              value={form.description}
              onChange={(e) => setForm({ ...form, description: e.target.value })}
              rows={3}
              className="w-full border border-outline-variant rounded-lg px-md py-sm"
            />
          </div>
          <LoadingButton
            type="submit"
            loading={saving}
            loadingText="Saving…"
            className="w-full bg-secondary-container text-white py-sm rounded-lg font-bold disabled:opacity-50"
          >
            Add Category
          </LoadingButton>
        </form>

        <div className="lg:col-span-2 bg-white rounded-xl border border-outline-variant shadow-sm overflow-hidden">
          <div className="p-lg border-b border-outline-variant">
            <h2 className="font-headline-sm text-headline-sm font-bold">All Categories</h2>
          </div>
          {loading ? (
            <div className="p-lg space-y-md loading-enter" aria-hidden="true">
              {Array.from({ length: 4 }).map((_, i) => (
                <Skeleton key={i} className="h-16 w-full rounded-lg" />
              ))}
            </div>
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
                        maxLength={200}
                        value={editing.title}
                        onChange={(e) => setEditing({ ...editing, title: e.target.value })}
                        className="w-full border border-outline-variant rounded-lg px-md py-sm"
                      />
                      <textarea
                        required
                        maxLength={500}
                        value={editing.description}
                        onChange={(e) => setEditing({ ...editing, description: e.target.value })}
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
                        <p className="text-sm text-on-surface-variant mt-xs">{cat.description}</p>
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
    </>
  );
}
