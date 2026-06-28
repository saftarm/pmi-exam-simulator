import StatusBadge from './StatusBadge';
import { TableSkeleton } from '../loading';
import { formatDate, getInitials } from '../../utils/userDisplay';
import { USER_STATUSES } from './EditUserModal';

function statusBadgeType(status) {
  if (status === 'Active') return 'success';
  if (status === 'Suspended') return 'error';
  return 'warning';
}

export default function UsersTable({ users, loading, onEdit, onStatusChange }) {
  return (
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
                  <StatusBadge status={user.status} type={statusBadgeType(user.status)} />
                </td>
                <td className="px-lg py-md text-right space-x-sm">
                  <button
                    type="button"
                    onClick={() => onEdit(user)}
                    className="text-sm font-bold text-secondary-container hover:underline"
                  >
                    Edit
                  </button>
                  <select
                    value={user.status}
                    onChange={(e) => onStatusChange(user, e.target.value)}
                    className="text-xs border border-outline-variant rounded px-sm py-xs"
                    aria-label={`Change status for ${user.displayName}`}
                  >
                    {USER_STATUSES.map((s) => (
                      <option key={s} value={s}>
                        {s}
                      </option>
                    ))}
                  </select>
                </td>
              </tr>
            ))
          )}
        </tbody>
      </table>
    </div>
  );
}
