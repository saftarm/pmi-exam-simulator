import { useEffect, useMemo, useState } from 'react';
import AdminLayout from '../../components/admin/AdminLayout';
import { StatCard } from '../../components/admin/StatusBadge';
import StatusBadge from '../../components/admin/StatusBadge';
import { getAllExams } from '../../services/adminExamService';
import { getCategories } from '../../services/adminCategoryService';
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
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        Promise.all([
            getUserCount().catch(() => 0),
            getAllExams().catch(() => []),
            getCategories().catch(() => []),
            getUserStats().catch(() => ({ byRole: {}, byStatus: {} })),
        ])
            .then(([count, examData, categories, stats]) => {
                setTotalUsers(count);
                setExams(examData);
                setCategoryCount(categories.length);
                setUsersByRole(stats.byRole ?? {});
                setUsersByStatus(stats.byStatus ?? {});
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

    return (
        <AdminLayout title="Reports">
            <div className="mb-lg p-md bg-surface-container-low border border-outline-variant rounded-lg text-sm text-on-surface-variant">
                Revenue, attempt volume, and pass-rate analytics require a future backend API. Below is live data from existing endpoints.
            </div>

            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-lg mb-xl">
                <StatCard
                    label="Total Users"
                    value={totalUsers.toLocaleString()}
                    change="API"
                    icon="group"
                    iconBg="bg-blue-50"
                    iconColor="text-primary"
                    loading={loading}
                />
                <StatCard
                    label="Total Exams"
                    value={String(exams.length)}
                    change="API"
                    icon="quiz"
                    iconBg="bg-green-50"
                    iconColor="text-green-600"
                    loading={loading}
                />
                <StatCard
                    label="Published Exams"
                    value={String(publishedExams)}
                    change="Live"
                    icon="published_with_changes"
                    iconBg="bg-amber-50"
                    iconColor="text-amber-600"
                    loading={loading}
                />
                <StatCard
                    label="Categories"
                    value={String(categoryCount)}
                    change="API"
                    icon="category"
                    iconBg="bg-purple-50"
                    iconColor="text-purple-600"
                    loading={loading}
                />
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
                    <h2 className="font-headline-sm text-headline-sm font-bold mb-lg">Users by status</h2>
                    <ul className="space-y-sm">
                        {STATUSES.map((status) => (
                            <li key={status} className="flex justify-between text-sm">
                                <span className="text-on-surface-variant">{status}</span>
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
                                    <td className="px-lg py-md text-on-surface-variant">{formatDate(user.createdAt)}</td>
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

            <div className="mt-lg p-md bg-amber-50 border border-amber-200 rounded-lg text-sm text-amber-800">
                Planned — needs API: revenue trends, exam attempt counts, average pass rate, server activity log.
            </div>
        </AdminLayout>
    );
}
