import LoadingButton from '../loading/LoadingButton';
import { USER_ROLES, USER_STATUSES } from './EditUserModal';

export default function CreateUserForm({ form, saving, onChange, onSubmit }) {
  return (
    <form
      onSubmit={onSubmit}
      className="bg-white rounded-xl border border-outline-variant p-lg mb-lg grid grid-cols-1 md:grid-cols-3 lg:grid-cols-6 gap-md items-end"
    >
      <input
        required
        placeholder="First name"
        value={form.firstName}
        onChange={(e) => onChange({ ...form, firstName: e.target.value })}
        className="border border-outline-variant rounded-lg px-md py-sm"
      />
      <input
        required
        placeholder="Username"
        value={form.userName}
        onChange={(e) => onChange({ ...form, userName: e.target.value })}
        className="border border-outline-variant rounded-lg px-md py-sm"
      />
      <input
        required
        type="email"
        placeholder="Email"
        value={form.email}
        onChange={(e) => onChange({ ...form, email: e.target.value })}
        className="border border-outline-variant rounded-lg px-md py-sm"
      />
      <input
        required
        type="password"
        placeholder="Password"
        value={form.password}
        onChange={(e) => onChange({ ...form, password: e.target.value })}
        className="border border-outline-variant rounded-lg px-md py-sm"
      />
      <select
        value={form.role}
        onChange={(e) => onChange({ ...form, role: e.target.value })}
        className="border border-outline-variant rounded-lg px-md py-sm"
      >
        {USER_ROLES.map((r) => (
          <option key={r} value={r}>
            {r}
          </option>
        ))}
      </select>
      <select
        value={form.status}
        onChange={(e) => onChange({ ...form, status: e.target.value })}
        className="border border-outline-variant rounded-lg px-md py-sm"
        aria-label="Account status"
      >
        {USER_STATUSES.map((s) => (
          <option key={s} value={s}>
            {s}
          </option>
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
  );
}
