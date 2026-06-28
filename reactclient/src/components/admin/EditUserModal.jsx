import { useState } from 'react';
import LoadingButton from '../loading/LoadingButton';

export const USER_ROLES = ['Learner', 'Pro', 'Admin'];
export const USER_STATUSES = ['Active', 'Suspended', 'Pending'];

export default function EditUserModal({ user, saving, onClose, onSave }) {
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
              {USER_ROLES.map((r) => (
                <option key={r} value={r}>
                  {r}
                </option>
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
              {USER_STATUSES.map((s) => (
                <option key={s} value={s}>
                  {s}
                </option>
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
