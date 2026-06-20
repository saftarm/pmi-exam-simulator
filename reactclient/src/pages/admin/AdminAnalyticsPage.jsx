import { useEffect, useState } from 'react';
import AdminLayout from '../../components/admin/AdminLayout';
import { StatCard } from '../../components/admin/StatusBadge';
import { getAnalytics, saveAnalytics, getActivity } from '../../services/adminMockStore';
import { getAllExams } from '../../services/adminExamService';
import { isPublishedExam } from '../../utils/examStatus';

export default function AdminAnalyticsPage() {
    const [analytics, setAnalytics] = useState(() => getAnalytics());
    const [activity, setActivity] = useState(() => getActivity());
    const [activeExams, setActiveExams] = useState(0);

    useEffect(() => {
        getAllExams()
            .then((exams) => setActiveExams(exams.filter(isPublishedExam).length))
            .catch(() => setActiveExams(0));
    }, []);

    const maxTrend = Math.max(...analytics.revenueTrend, 1);

    const handlePassRateChange = (value) => {
        const next = { ...analytics, passRate: Number(value) };
        setAnalytics(next);
        saveAnalytics(next);
    };

    return (
        <AdminLayout title="Analytics">
            <div className="mb-lg p-md bg-amber-50 border border-amber-200 rounded-lg text-sm text-amber-800">
                Revenue and attempt metrics are demo data. Active exams count is loaded from the API.
            </div>

            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-lg mb-xl">
                <StatCard
                    label="Monthly Revenue"
                    value={`$${analytics.monthlyRevenue.toLocaleString()}`}
                    change="+8.4%"
                    icon="payments"
                    iconBg="bg-green-50"
                    iconColor="text-green-600"
                />
                <StatCard
                    label="Exam Attempts"
                    value={analytics.attemptsThisMonth.toLocaleString()}
                    change="This month"
                    changeType="negative"
                    icon="assignment"
                    iconBg="bg-blue-50"
                    iconColor="text-primary"
                />
                <StatCard
                    label="Avg. Pass Rate"
                    value={`${analytics.passRate}%`}
                    change="Adjustable demo"
                    changeType="negative"
                    icon="trending_up"
                    iconBg="bg-amber-50"
                    iconColor="text-amber-600"
                />
                <StatCard
                    label="Active Exams (API)"
                    value={String(activeExams)}
                    change="Published"
                    icon="quiz"
                    iconBg="bg-purple-50"
                    iconColor="text-purple-600"
                />
            </div>

            <div className="grid grid-cols-1 lg:grid-cols-2 gap-lg">
                <div className="bg-white rounded-xl border border-outline-variant shadow-sm p-lg">
                    <h2 className="font-headline-sm text-headline-sm font-bold mb-lg">Revenue Trend (demo)</h2>
                    <div className="flex items-end gap-sm h-40">
                        {analytics.revenueTrend.map((val, i) => (
                            <div key={i} className="flex-1 flex flex-col items-center gap-xs">
                                <div
                                    className="w-full bg-secondary-container rounded-t"
                                    style={{ height: `${(val / maxTrend) * 100}%`, minHeight: 4 }}
                                />
                                <span className="text-xs text-on-surface-variant">W{i + 1}</span>
                            </div>
                        ))}
                    </div>
                </div>

                <div className="bg-white rounded-xl border border-outline-variant shadow-sm p-lg">
                    <h2 className="font-headline-sm text-headline-sm font-bold mb-md">Pass Rate Target</h2>
                    <p className="text-sm text-on-surface-variant mb-md">
                        Demo control for dashboard avg. pass rate display.
                    </p>
                    <input
                        type="range"
                        min={0}
                        max={100}
                        step={0.1}
                        value={analytics.passRate}
                        onChange={(e) => handlePassRateChange(e.target.value)}
                        className="w-full"
                    />
                    <p className="mt-sm font-bold text-primary">{analytics.passRate}%</p>
                    <p className="mt-lg text-sm text-on-surface-variant">
                        Completion rate: <strong>{analytics.completionRate}%</strong>
                    </p>
                </div>

                <div className="lg:col-span-2 bg-white rounded-xl border border-outline-variant shadow-sm overflow-hidden">
                    <div className="p-lg border-b border-outline-variant">
                        <h2 className="font-headline-sm text-headline-sm font-bold">Activity Log</h2>
                    </div>
                    <div className="overflow-x-auto max-h-80 overflow-y-auto">
                        <table className="w-full text-left text-sm">
                            <thead className="bg-surface-container-low text-on-surface-variant text-xs uppercase sticky top-0">
                                <tr>
                                    <th className="px-lg py-md">User</th>
                                    <th className="px-lg py-md">Action</th>
                                    <th className="px-lg py-md">Time</th>
                                    <th className="px-lg py-md">Status</th>
                                </tr>
                            </thead>
                            <tbody className="divide-y divide-outline-variant">
                                {activity.map((row) => (
                                    <tr key={row.id}>
                                        <td className="px-lg py-md">{row.user}</td>
                                        <td className="px-lg py-md text-on-surface-variant">{row.action}</td>
                                        <td className="px-lg py-md text-on-surface-variant">{row.timestamp}</td>
                                        <td className="px-lg py-md">{row.status}</td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </AdminLayout>
    );
}
