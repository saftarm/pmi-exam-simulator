import { useEffect, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import AdminLayout from '../../components/admin/AdminLayout';
import { StatCard } from '../../components/admin/StatusBadge';
import StatusBadge from '../../components/admin/StatusBadge';
import Icon from '../../components/Icon';
import { getAllExams } from '../../services/adminExamService';
import { getActivity, getAvgPassRate, getTotalUsers } from '../../services/adminMockStore';
import { isPublishedExam } from '../../utils/examStatus';

export default function AdminOverviewPage() {
    const navigate = useNavigate();
    const [exams, setExams] = useState([]);
    const [loading, setLoading] = useState(true);
    const [activity] = useState(() => getActivity());

    useEffect(() => {
        getAllExams()
            .then(setExams)
            .catch(() => setExams([]))
            .finally(() => setLoading(false));
    }, []);

    const activeExams = exams.filter(isPublishedExam).length;
    const totalUsers = getTotalUsers();
    const passRate = getAvgPassRate();

    return (
        <AdminLayout title="Dashboard Overview">
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-lg mb-xl">
                <StatCard
                    label="Total Users"
                    value={loading ? '…' : totalUsers.toLocaleString()}
                    change="+12.5%"
                    icon="group"
                    iconBg="bg-blue-50"
                    iconColor="text-primary"
                />
                <StatCard
                    label="Active Exams"
                    value={loading ? '…' : String(activeExams)}
                    change="+2"
                    icon="quiz"
                    iconBg="bg-green-50"
                    iconColor="text-green-600"
                />
                <StatCard
                    label="Avg. Pass Rate"
                    value={`${passRate}%`}
                    change="-1.2%"
                    changeType="negative"
                    icon="trending_up"
                    iconBg="bg-amber-50"
                    iconColor="text-amber-600"
                />
                <StatCard
                    label="Total Exams"
                    value={loading ? '…' : String(exams.length)}
                    change={`${exams.length} in catalog`}
                    changeType="negative"
                    icon="library_books"
                    iconBg="bg-purple-50"
                    iconColor="text-purple-600"
                />
            </div>

            <div className="grid grid-cols-1 lg:grid-cols-3 gap-lg">
                <div className="lg:col-span-2 bg-white rounded-xl border border-outline-variant shadow-sm overflow-hidden">
                    <div className="p-lg border-b border-outline-variant flex justify-between items-center">
                        <h2 className="font-headline-sm text-headline-sm font-bold">Recent Activity</h2>
                        <Link to="/admin/analytics" className="text-secondary-container text-sm font-bold hover:underline">
                            View All
                        </Link>
                    </div>
                    <div className="overflow-x-auto">
                        <table className="w-full text-left">
                            <thead className="bg-surface-container-low text-on-surface-variant text-xs uppercase tracking-wider">
                                <tr>
                                    <th className="px-lg py-md font-semibold">User</th>
                                    <th className="px-lg py-md font-semibold">Action</th>
                                    <th className="px-lg py-md font-semibold">Time</th>
                                    <th className="px-lg py-md font-semibold">Status</th>
                                </tr>
                            </thead>
                            <tbody className="divide-y divide-outline-variant">
                                {activity.slice(0, 6).map((row) => (
                                    <tr key={row.id} className="hover:bg-surface-container-low/50 transition-colors">
                                        <td className="px-lg py-md">
                                            <div className="flex items-center gap-md">
                                                <div className="w-8 h-8 rounded-full bg-primary/10 flex items-center justify-center text-xs font-bold text-primary">
                                                    {row.initials}
                                                </div>
                                                <span className="font-medium">{row.user}</span>
                                            </div>
                                        </td>
                                        <td className="px-lg py-md text-on-surface-variant">{row.action}</td>
                                        <td className="px-lg py-md text-on-surface-variant text-sm">{row.timestamp}</td>
                                        <td className="px-lg py-md">
                                            <StatusBadge status={row.status} type={row.statusType} />
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>
                </div>

                <div className="space-y-lg">
                    <div className="bg-white p-lg rounded-xl border border-outline-variant shadow-sm">
                        <h2 className="font-headline-sm text-headline-sm font-bold mb-lg">Quick Actions</h2>
                        <div className="grid grid-cols-2 gap-md">
                            <button
                                type="button"
                                onClick={() => navigate('/admin/exams/new')}
                                className="flex flex-col items-center justify-center p-md border border-outline-variant rounded-lg hover:bg-surface-container-low transition-colors group"
                            >
                                <Icon name="add_circle" className="text-secondary-container mb-sm group-hover:scale-110 transition-transform" />
                                <span className="text-xs font-bold">Create Exam</span>
                            </button>
                            <button
                                type="button"
                                onClick={() => navigate('/admin/users')}
                                className="flex flex-col items-center justify-center p-md border border-outline-variant rounded-lg hover:bg-surface-container-low transition-colors group"
                            >
                                <Icon name="person_add" className="text-secondary-container mb-sm group-hover:scale-110 transition-transform" />
                                <span className="text-xs font-bold">Add User</span>
                            </button>
                            <button
                                type="button"
                                onClick={() => navigate('/admin/categories')}
                                className="flex flex-col items-center justify-center p-md border border-outline-variant rounded-lg hover:bg-surface-container-low transition-colors group"
                            >
                                <Icon name="category" className="text-secondary-container mb-sm group-hover:scale-110 transition-transform" />
                                <span className="text-xs font-bold">Categories</span>
                            </button>
                            <button
                                type="button"
                                onClick={() => navigate('/admin/analytics')}
                                className="flex flex-col items-center justify-center p-md border border-outline-variant rounded-lg hover:bg-surface-container-low transition-colors group"
                            >
                                <Icon name="analytics" className="text-secondary-container mb-sm group-hover:scale-110 transition-transform" />
                                <span className="text-xs font-bold">Analytics</span>
                            </button>
                        </div>
                    </div>

                    <div className="bg-primary p-lg rounded-xl text-white shadow-lg relative overflow-hidden">
                        <div className="relative z-10">
                            <h2 className="font-headline-sm text-headline-sm font-bold mb-sm">System Status</h2>
                            <p className="text-sm opacity-80 mb-lg">
                                {activeExams} published exam{activeExams !== 1 ? 's' : ''} live for learners.
                            </p>
                            <button
                                type="button"
                                onClick={() => navigate('/admin/settings')}
                                className="w-full bg-secondary-container hover:bg-secondary text-white py-sm rounded-lg font-bold text-sm transition-colors"
                            >
                                Open Settings
                            </button>
                        </div>
                        <Icon
                            name="settings"
                            className="absolute -right-4 -bottom-4 opacity-10"
                            style={{ fontSize: 120 }}
                        />
                    </div>
                </div>
            </div>
        </AdminLayout>
    );
}
