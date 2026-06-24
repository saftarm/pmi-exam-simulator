import { useEffect, useMemo, useState } from 'react';
import AdminLayout from '../../components/admin/AdminLayout';
import { StatCard } from '../../components/admin/StatusBadge';
import StatusBadge from '../../components/admin/StatusBadge';
import { getAllExams, getExamOverviewStats } from '../../services/adminExamService';
import { getCategories } from '../../services/adminCategoryService';
import { getAttemptVolume, getPassRateAnalytics } from '../../services/adminAnalyticsService';
import { getUserCount, getUserStats, getUsers } from '../../services/adminUserService';
import { isPublishedExam } from '../../utils/examStatus';
import { formatDate, getInitials } from '../../utils/userDisplay';
import { TableSkeleton } from '../../components/loading';

const ROLES = ['Learner', 'Pro', 'Admin'];
const STATUSES = ['Active', 'Suspended', 'Pending'];

export default function AdminAnalyticsPage() {
  const [totalUsers, setTotalUsers] = useState(0);
  const [exams, setExams] = useState([]);
  const [categoryCount, setCategoryCount] = useState(0);
  const [usersByRole, setUsersByRole] = useState({});
  const [usersByStatus, setUsersByStatus] = useState({});
  const [recentUsers, setRecentUsers] = useState([]);
  const [userPage, setUserPage] = useState(1);
  const [userTotal, setUserTotal] = useState(0);
  const [hasNextUsers, setHasNextUsers] = useState(false);
  const [attemptVolume, setAttemptVolume] = useState([]);
  const [passRate, setPassRate] = useState(null);
  const [examStats, setExamStats] = useState([]);
  const [loading, setLoading] = useState(true);
  const [loadError, setLoadError] = useState(null);

  useEffect(() => {
    let failed = false;
    Promise.all([
      getUserCount().catch(() => {
        failed = true;
        return 0;
      }),
      getAllExams().catch(() => {
        failed = true;
        return [];
      }),
      getCategories().catch(() => {
        failed = true;
        return [];
      }),
      getUserStats().catch(() => {
        failed = true;
        return { byRole: {}, byStatus: {} };
      }),
      getAttemptVolume(30).catch(() => {
        failed = true;
        return [];
      }),
      getPassRateAnalytics().catch(() => {
        failed = true;
        return null;
      }),
      getExamOverviewStats().catch(() => {
        failed = true;
        return [];
      }),
    ])
      .then(([count, examData, categories, stats, volume, pass, overview]) => {
        setTotalUsers(count);
        setExams(examData);
        setCategoryCount(categories.length);
        setUsersByRole(stats.byRole ?? {});
        setUsersByStatus(stats.byStatus ?? {});
        setAttemptVolume(volume);
        setPassRate(pass);
        setExamStats(overview);
        if (failed) {
          setLoadError('Could not load all report data. Showing partial results.');
        }
      })
      .finally(() => setLoading(false));
  }, []);

  useEffect(() => {
    getUsers({ pageNumber: userPage, pageSize: 20 })
      .then((data) => {
        setRecentUsers(data.items || []);
        setUserTotal(data.totalCount ?? 0);
        setHasNextUsers(data.hasNextPage ?? false);
      })
      .catch(() => {
        setRecentUsers([]);
      });
  }, [userPage]);

  const publishedExams = useMemo(() => exams.filter(isPublishedExam).length, [exams]);
  const totalAttempts30d = useMemo(
    () => attemptVolume.reduce((sum, row) => sum + (row.count ?? 0), 0),
    [attemptVolume],
  );
  const maxVolume = useMemo(
    () => Math.max(...attemptVolume.map((r) => r.count ?? 0), 1),
    [attemptVolume],
  );

  return (
    <AdminLayout title="Reports">
      {loadError && (
        <div className="mb-lg p-md bg-amber-50 text-amber-900 rounded-lg border border-amber-200 flex justify-between gap-md">
          <span>{loadError}</span>
          <button type="button" onClick={() => setLoadError(null)} className="shrink-0 font-bold">
            Dismiss
          </button>
        </div>
      )}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-lg mb-xl">
        <StatCard
          label="Total Users"
          value={totalUsers.toLocaleString()}
          change="Live"
          icon="group"
          iconBg="bg-blue-50"
          iconColor="text-primary"
          loading={loading}
        />
        <StatCard
          label="Attempts (30d)"
          value={String(totalAttempts30d)}
          change="Live"
          icon="history"
          iconBg="bg-green-50"
          iconColor="text-green-600"
          loading={loading}
        />
        <StatCard
          label="Pass Rate"
          value={passRate ? `${passRate.passRate}%` : '—'}
          change={passRate ? `≥${passRate.passThreshold}%` : ''}
          icon="verified"
          iconBg="bg-amber-50"
          iconColor="text-amber-600"
          loading={loading}
        />
        <StatCard
          label="Avg Score"
          value={passRate ? `${passRate.averageScore}%` : '—'}
          change={passRate ? `${passRate.totalCompletedAttempts} attempts` : ''}
          icon="query_stats"
          iconBg="bg-purple-50"
          iconColor="text-purple-600"
          loading={loading}
        />
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-lg mb-lg">
        <div className="bg-white rounded-xl border border-outline-variant shadow-sm p-lg">
          <h2 className="font-headline-sm text-headline-sm font-bold mb-lg">
            Attempt volume (30 days)
          </h2>
          {attemptVolume.length === 0 ? (
            <p className="text-sm text-on-surface-variant">No completed attempts in this period.</p>
          ) : (
            <div className="flex items-end gap-1 h-32">
              {attemptVolume.map((row) => (
                <div
                  key={row.date}
                  className="flex-1 bg-secondary-container/80 rounded-t-sm min-h-[2px]"
                  style={{ height: `${((row.count ?? 0) / maxVolume) * 100}%` }}
                  title={`${row.date}: ${row.count} attempts`}
                />
              ))}
            </div>
          )}
        </div>
        <div className="bg-white rounded-xl border border-outline-variant shadow-sm p-lg">
          <h2 className="font-headline-sm text-headline-sm font-bold mb-lg">Exam popularity</h2>
          {examStats.length === 0 ? (
            <p className="text-sm text-on-surface-variant">No attempt data yet.</p>
          ) : (
            <ul className="space-y-sm max-h-40 overflow-y-auto">
              {examStats.slice(0, 8).map((row) => (
                <li key={row.examId} className="flex justify-between text-sm">
                  <span className="text-on-surface-variant truncate pr-md">{row.examTitle}</span>
                  <span className="font-bold shrink-0">{row.attemptCount} attempts</span>
                </li>
              ))}
            </ul>
          )}
        </div>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-lg mb-lg">
        <div className="bg-white rounded-xl border border-outline-variant shadow-sm p-lg">
          <h2 className="font-headline-sm text-headline-sm font-bold mb-lg">Users by role</h2>
          <ul className="space-y-sm">
            {ROLES.map((role) => (
              <li key={role} className="flex justify-between text-sm">
                <span className="text-on-surface-variant">{role}</span>
                <span className="font-bold">{usersByRole[role] ?? 0}</span>
              </li>
            ))}
          </ul>
        </div>
        <div className="bg-white rounded-xl border border-outline-variant shadow-sm p-lg">
          <h2 className="font-headline-sm text-headline-sm font-bold mb-lg">Catalog summary</h2>
          <ul className="space-y-sm">
            <li className="flex justify-between text-sm">
              <span className="text-on-surface-variant">Total exams</span>
              <span className="font-bold">{exams.length}</span>
            </li>
            <li className="flex justify-between text-sm">
              <span className="text-on-surface-variant">Published exams</span>
              <span className="font-bold">{publishedExams}</span>
            </li>
            <li className="flex justify-between text-sm">
              <span className="text-on-surface-variant">Categories</span>
              <span className="font-bold">{categoryCount}</span>
            </li>
            {STATUSES.map((status) => (
              <li key={status} className="flex justify-between text-sm">
                <span className="text-on-surface-variant">Users · {status}</span>
                <span className="font-bold">{usersByStatus[status] ?? 0}</span>
              </li>
            ))}
          </ul>
        </div>
      </div>

      <div className="bg-white rounded-xl border border-outline-variant shadow-sm overflow-hidden">
        <div className="p-lg border-b border-outline-variant">
          <h2 className="font-headline-sm text-headline-sm font-bold">Recent users</h2>
        </div>
        <div className="overflow-x-auto">
          <table className="w-full text-left text-sm">
            <thead className="bg-surface-container-low text-on-surface-variant text-xs uppercase">
              <tr>
                <th className="px-lg py-md">User</th>
                <th className="px-lg py-md">Email</th>
                <th className="px-lg py-md">Role</th>
                <th className="px-lg py-md">Joined</th>
                <th className="px-lg py-md">Status</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-outline-variant">
              {recentUsers.map((user) => (
                <tr key={user.id}>
                  <td className="px-lg py-md">
                    <div className="flex items-center gap-sm">
                      <div className="w-8 h-8 rounded-full bg-primary/10 flex items-center justify-center text-xs font-bold text-primary">
                        {getInitials(user.displayName)}
                      </div>
                      {user.displayName}
                    </div>
                  </td>
                  <td className="px-lg py-md text-on-surface-variant">{user.email}</td>
                  <td className="px-lg py-md">{user.role}</td>
                  <td className="px-lg py-md text-on-surface-variant">
                    {formatDate(user.createdAt)}
                  </td>
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
                </tr>
              ))}
            </tbody>
          </table>
        </div>
        <div className="flex justify-between items-center p-lg border-t border-outline-variant text-sm">
          <span>{userTotal} users total</span>
          <div className="flex gap-md">
            <button
              type="button"
              disabled={userPage <= 1}
              onClick={() => setUserPage((p) => p - 1)}
              className="px-md py-sm border border-outline-variant rounded-lg disabled:opacity-50"
            >
              Previous
            </button>
            <span className="py-sm">Page {userPage}</span>
            <button
              type="button"
              disabled={!hasNextUsers}
              onClick={() => setUserPage((p) => p + 1)}
              className="px-md py-sm border border-outline-variant rounded-lg disabled:opacity-50"
            >
              Next
            </button>
          </div>
        </div>
      </div>
    </AdminLayout>
  );
}
