import { useEffect, useMemo, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import AdminLayout from '../../components/admin/AdminLayout';
import { StatCard } from '../../components/admin/StatusBadge';
import StatusBadge from '../../components/admin/StatusBadge';
import Icon from '../../components/Icon';
import { getAllExams, getExamOverviewStats } from '../../services/adminExamService';
import { getUserCount, getUsers } from '../../services/adminUserService';
import { isPublishedExam } from '../../utils/examStatus';
import { formatDate, getInitials } from '../../utils/userDisplay';
import { TableSkeleton } from '../../components/loading';

export default function AdminOverviewPage() {
  const navigate = useNavigate();
  const [exams, setExams] = useState([]);
  const [examStats, setExamStats] = useState([]);
  const [totalUsers, setTotalUsers] = useState(0);
  const [recentUsers, setRecentUsers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [loadError, setLoadError] = useState(null);

  useEffect(() => {
    let failed = false;
    Promise.all([
      getAllExams().catch(() => {
        failed = true;
        return [];
      }),
      getExamOverviewStats().catch(() => {
        failed = true;
        return [];
      }),
      getUserCount().catch(() => {
        failed = true;
        return 0;
      }),
      getUsers({ pageNumber: 1, pageSize: 6 }).catch(() => {
        failed = true;
        return { items: [] };
      }),
    ])
      .then(([examData, stats, count, usersData]) => {
        setExams(examData);
        setExamStats(stats);
        setTotalUsers(count);
        setRecentUsers(usersData.items || []);
        if (failed) {
          setLoadError('Could not load all dashboard data. Showing partial results.');
        }
      })
      .finally(() => setLoading(false));
  }, []);

  const activeExams = exams.filter(isPublishedExam).length;
  const draftExams = exams.length - activeExams;

  const rankedStats = useMemo(
    () => examStats.map((row, index) => ({ ...row, rank: index + 1 })),
    [examStats],
  );

  const maxAttempts = useMemo(
    () => Math.max(...examStats.map((s) => s.attemptCount), 1),
    [examStats],
  );

  return (
    <AdminLayout title="Dashboard Overview" showNewExam>
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
          change="Live from API"
          changeType="negative"
          icon="group"
          iconBg="bg-blue-50"
          iconColor="text-primary"
          loading={loading}
        />
        <StatCard
          label="Published Exams"
          value={String(activeExams)}
          change="Live for learners"
          changeType="negative"
          icon="quiz"
          iconBg="bg-green-50"
          iconColor="text-green-600"
          loading={loading}
        />
        <StatCard
          label="Draft Exams"
          value={String(draftExams)}
          change="Not published"
          changeType="negative"
          icon="edit_note"
          iconBg="bg-amber-50"
          iconColor="text-amber-600"
          loading={loading}
        />
        <StatCard
          label="Total Exams"
          value={String(exams.length)}
          change={`${exams.length} in catalog`}
          changeType="negative"
          icon="library_books"
          iconBg="bg-purple-50"
          iconColor="text-purple-600"
          loading={loading}
        />
      </div>

      <div className="bg-white rounded-xl border border-outline-variant shadow-sm overflow-hidden mb-xl">
        <div className="p-lg border-b border-outline-variant">
          <h2 className="font-headline-sm text-headline-sm font-bold">Exam Performance</h2>
          <p className="text-xs text-on-surface-variant mt-xs">
            Popularity, average score, and unique learners per exam (completed sessions only)
          </p>
        </div>
        <div className="overflow-x-auto">
          <table className="w-full text-left text-sm">
            <thead className="bg-surface-container-low text-on-surface-variant text-xs uppercase tracking-wider">
              <tr>
                <th className="px-lg py-md font-semibold">Rank</th>
                <th className="px-lg py-md font-semibold">Exam</th>
                <th className="px-lg py-md font-semibold">Popularity</th>
                <th className="px-lg py-md font-semibold">Attempts</th>
                <th className="px-lg py-md font-semibold">Learners</th>
                <th className="px-lg py-md font-semibold">Avg. Score</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-outline-variant">
              {loading ? (
                <TableSkeleton rows={4} columns={6} />
              ) : rankedStats.length === 0 ? (
                <tr>
                  <td colSpan={6} className="px-lg py-xl text-center text-on-surface-variant">
                    No exam attempt data yet.
                  </td>
                </tr>
              ) : (
                rankedStats.map((row) => (
                  <tr key={row.examId} className="hover:bg-surface-container-low/50">
                    <td className="px-lg py-md font-bold text-on-surface-variant">#{row.rank}</td>
                    <td className="px-lg py-md font-medium">{row.examTitle}</td>
                    <td className="px-lg py-md">
                      <div className="flex items-center gap-sm min-w-[120px]">
                        <div className="flex-1 h-2 bg-surface-container-low rounded-full overflow-hidden">
                          <div
                            className="h-full bg-secondary-container rounded-full transition-all"
                            style={{
                              width: `${(row.attemptCount / maxAttempts) * 100}%`,
                            }}
                          />
                        </div>
                        <span className="text-xs text-on-surface-variant w-8 text-right">
                          {row.attemptCount > 0
                            ? Math.round((row.attemptCount / maxAttempts) * 100)
                            : 0}
                          %
                        </span>
                      </div>
                    </td>
                    <td className="px-lg py-md">{row.attemptCount}</td>
                    <td className="px-lg py-md">{row.uniqueUsersCount}</td>
                    <td className="px-lg py-md">
                      <span className="font-bold text-primary">
                        {Number(row.averageScore).toFixed(1)}%
                      </span>
                    </td>
                  </tr>
                ))
              )}
            </tbody>
          </table>
        </div>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-lg">
        <div className="lg:col-span-2 bg-white rounded-xl border border-outline-variant shadow-sm overflow-hidden">
          <div className="p-lg border-b border-outline-variant flex justify-between items-center">
            <div>
              <h2 className="font-headline-sm text-headline-sm font-bold">Recent Registrations</h2>
              <p className="text-xs text-on-surface-variant mt-xs">Latest user sign-ups</p>
            </div>
            <Link
              to="/admin/analytics"
              className="text-secondary-container text-sm font-bold hover:underline"
            >
              View reports
            </Link>
          </div>
          <div className="overflow-x-auto">
            <table className="w-full text-left">
              <thead className="bg-surface-container-low text-on-surface-variant text-xs uppercase tracking-wider">
                <tr>
                  <th className="px-lg py-md font-semibold">User</th>
                  <th className="px-lg py-md font-semibold">Email</th>
                  <th className="px-lg py-md font-semibold">Joined</th>
                  <th className="px-lg py-md font-semibold">Status</th>
                </tr>
              </thead>
              <tbody className="divide-y divide-outline-variant">
                {loading ? (
                  <TableSkeleton rows={5} columns={4} />
                ) : recentUsers.length === 0 ? (
                  <tr>
                    <td colSpan={4} className="px-lg py-xl text-center text-on-surface-variant">
                      No users yet.
                    </td>
                  </tr>
                ) : (
                  recentUsers.map((user) => (
                    <tr
                      key={user.id}
                      className="hover:bg-surface-container-low/50 transition-colors"
                    >
                      <td className="px-lg py-md">
                        <div className="flex items-center gap-md">
                          <div className="w-8 h-8 rounded-full bg-primary/10 flex items-center justify-center text-xs font-bold text-primary">
                            {getInitials(user.displayName)}
                          </div>
                          <span className="font-medium">{user.displayName}</span>
                        </div>
                      </td>
                      <td className="px-lg py-md text-on-surface-variant">{user.email}</td>
                      <td className="px-lg py-md text-on-surface-variant text-sm">
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
                  ))
                )}
              </tbody>
            </table>
          </div>
        </div>

        <div className="bg-white p-lg rounded-xl border border-outline-variant shadow-sm h-fit">
          <h2 className="font-headline-sm text-headline-sm font-bold mb-lg">Quick Actions</h2>
          <div className="grid grid-cols-1 gap-md">
            <button
              type="button"
              onClick={() => navigate('/admin/users')}
              className="flex items-center gap-md p-md border border-outline-variant rounded-lg hover:bg-surface-container-low transition-colors group text-left"
            >
              <Icon
                name="person_add"
                className="text-secondary-container group-hover:scale-110 transition-transform"
              />
              <div>
                <span className="text-sm font-bold block">Add User</span>
                <span className="text-xs text-on-surface-variant">Manage learner accounts</span>
              </div>
            </button>
            <button
              type="button"
              onClick={() => navigate('/admin/questions')}
              className="flex items-center gap-md p-md border border-outline-variant rounded-lg hover:bg-surface-container-low transition-colors group text-left"
            >
              <Icon
                name="library_books"
                className="text-secondary-container group-hover:scale-110 transition-transform"
              />
              <div>
                <span className="text-sm font-bold block">Question Pool</span>
                <span className="text-xs text-on-surface-variant">Import or edit questions</span>
              </div>
            </button>
          </div>
        </div>
      </div>
    </AdminLayout>
  );
}
