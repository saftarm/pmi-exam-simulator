import { useEffect, useState } from 'react';
import { Link, useNavigate, useSearchParams } from 'react-router-dom';
import AdminLayout from '../../components/admin/AdminLayout';
import StatusBadge from '../../components/admin/StatusBadge';
import DeleteConfirmModal from '../../components/admin/DeleteConfirmModal';
import CategoryManagement from '../../components/admin/CategoryManagement';
import Icon from '../../components/Icon';
import { deleteExam, deleteExamsBulk, getAllExams, publishExam } from '../../services/adminExamService';
import { formatExamStatus, statusBadgeType } from '../../utils/examStatus';
import { TableSkeleton, Skeleton, Spinner } from '../../components/loading';

const TABS = [
    { id: 'exams', label: 'Exams' },
    { id: 'categories', label: 'Categories' },
];

export default function AdminExamsPage() {
    const navigate = useNavigate();
    const [searchParams, setSearchParams] = useSearchParams();
    const activeTab = searchParams.get('tab') === 'categories' ? 'categories' : 'exams';

    const [exams, setExams] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [selected, setSelected] = useState(new Set());
    const [deleteTarget, setDeleteTarget] = useState(null);
    const [deleting, setDeleting] = useState(false);
    const [publishingId, setPublishingId] = useState(null);

    const loadExams = () => {
        setLoading(true);
        getAllExams()
            .then(setExams)
            .catch((err) => setError(err.message || 'Failed to load exams'))
            .finally(() => setLoading(false));
    };

    useEffect(() => {
        if (activeTab === 'exams') {
            loadExams();
        }
    }, [activeTab]);

    const setTab = (tabId) => {
        if (tabId === 'exams') {
            setSearchParams({});
        } else {
            setSearchParams({ tab: tabId });
        }
    };

    const toggleSelect = (id) => {
        setSelected((prev) => {
            const next = new Set(prev);
            if (next.has(id)) next.delete(id);
            else next.add(id);
            return next;
        });
    };

    const toggleAll = () => {
        if (selected.size === exams.length) setSelected(new Set());
        else setSelected(new Set(exams.map((e) => e.id)));
    };

    const handlePublish = async (exam) => {
        setPublishingId(exam.id);
        try {
            await publishExam(exam.id);
            loadExams();
        } catch (err) {
            alert(err.response?.data?.title || err.message || 'Publish failed');
        } finally {
            setPublishingId(null);
        }
    };

    const handleDelete = async () => {
        if (!deleteTarget) return;
        setDeleting(true);
        try {
            if (deleteTarget.bulk) {
                await deleteExamsBulk([...selected]);
                setSelected(new Set());
            } else {
                await deleteExam(deleteTarget.id);
            }
            setDeleteTarget(null);
            loadExams();
        } catch (err) {
            alert(err.message || 'Delete failed');
        } finally {
            setDeleting(false);
        }
    };

    return (
        <AdminLayout title="Manage Exams">
            <div className="flex gap-sm mb-lg border-b border-outline-variant">
                {TABS.map((tab) => (
                    <button
                        key={tab.id}
                        type="button"
                        onClick={() => setTab(tab.id)}
                        className={`px-lg py-sm font-label-lg text-label-lg border-b-2 -mb-px transition-colors ${
                            activeTab === tab.id
                                ? 'border-secondary-container text-primary font-bold'
                                : 'border-transparent text-on-surface-variant hover:text-primary'
                        }`}
                    >
                        {tab.label}
                    </button>
                ))}
            </div>

            {activeTab === 'categories' ? (
                <CategoryManagement />
            ) : (
                <>
                    <div className="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-md mb-lg">
                        <p className="text-on-surface-variant">
                            {loading ? (
                                <Skeleton className="h-4 w-40 inline-block" />
                            ) : (
                                `${exams.length} exam${exams.length !== 1 ? 's' : ''} in catalog`
                            )}
                        </p>
                        <div className="flex gap-md">
                            {selected.size > 0 && (
                                <button
                                    type="button"
                                    onClick={() => setDeleteTarget({ bulk: true })}
                                    className="px-md py-sm rounded-lg border border-red-200 text-red-600 hover:bg-red-50 text-sm font-bold"
                                >
                                    Delete selected ({selected.size})
                                </button>
                            )}
                            <button
                                type="button"
                                onClick={() => navigate('/admin/exams/new')}
                                className="bg-secondary-container text-white px-md py-sm rounded-lg font-label-lg text-label-lg flex items-center gap-sm"
                            >
                                <Icon name="add" style={{ fontSize: 18 }} />
                                Create Exam
                            </button>
                        </div>
                    </div>

                    {error && (
                        <div className="mb-lg p-md bg-red-50 text-red-700 rounded-lg border border-red-200">{error}</div>
                    )}

                    <div className="bg-white rounded-xl border border-outline-variant shadow-sm overflow-hidden">
                        <div className="overflow-x-auto">
                            <table className="w-full text-left">
                                <thead className="bg-surface-container-low text-on-surface-variant text-xs uppercase tracking-wider">
                                    <tr>
                                        <th className="px-lg py-md w-10">
                                            <input
                                                type="checkbox"
                                                checked={exams.length > 0 && selected.size === exams.length}
                                                onChange={toggleAll}
                                                className="rounded border-outline-variant"
                                            />
                                        </th>
                                        <th className="px-lg py-md font-semibold">Title</th>
                                        <th className="px-lg py-md font-semibold">Status</th>
                                        <th className="px-lg py-md font-semibold">Questions</th>
                                        <th className="px-lg py-md font-semibold">Duration</th>
                                        <th className="px-lg py-md font-semibold text-right">Actions</th>
                                    </tr>
                                </thead>
                                <tbody className="divide-y divide-outline-variant">
                                    {loading ? (
                                        <TableSkeleton rows={5} columns={6} />
                                    ) : exams.length === 0 ? (
                                        <tr>
                                            <td colSpan={6} className="px-lg py-xl text-center text-on-surface-variant">
                                                No exams yet.{' '}
                                                <Link to="/admin/exams/new" className="text-secondary-container font-bold hover:underline">
                                                    Create one
                                                </Link>
                                            </td>
                                        </tr>
                                    ) : (
                                        exams.map((exam) => (
                                            <tr key={exam.id} className="hover:bg-surface-container-low/50">
                                                <td className="px-lg py-md">
                                                    <input
                                                        type="checkbox"
                                                        checked={selected.has(exam.id)}
                                                        onChange={() => toggleSelect(exam.id)}
                                                        className="rounded border-outline-variant"
                                                    />
                                                </td>
                                                <td className="px-lg py-md">
                                                    <Link
                                                        to={`/admin/exams/${exam.id}`}
                                                        className="font-medium text-primary hover:text-secondary-container"
                                                    >
                                                        {exam.title}
                                                    </Link>
                                                </td>
                                                <td className="px-lg py-md">
                                                    <StatusBadge
                                                        status={formatExamStatus(exam.status)}
                                                        type={statusBadgeType(exam.status)}
                                                    />
                                                </td>
                                                <td className="px-lg py-md text-on-surface-variant">
                                                    {exam.numberOfQuestions}
                                                </td>
                                                <td className="px-lg py-md text-on-surface-variant">
                                                    {exam.durationInMinutes} min
                                                </td>
                                                <td className="px-lg py-md">
                                                    <div className="flex justify-end gap-sm">
                                                        {formatExamStatus(exam.status) !== 'Published' && (
                                                            <button
                                                                type="button"
                                                                disabled={publishingId === exam.id}
                                                                onClick={() => handlePublish(exam)}
                                                                className="text-xs font-bold text-green-600 hover:underline disabled:opacity-50 inline-flex items-center gap-xs"
                                                            >
                                                                {publishingId === exam.id && <Spinner size="sm" label="Publishing" />}
                                                                Publish
                                                            </button>
                                                        )}
                                                        <Link
                                                            to={`/admin/exams/${exam.id}/edit`}
                                                            className="text-xs font-bold text-secondary-container hover:underline"
                                                        >
                                                            Edit
                                                        </Link>
                                                        <button
                                                            type="button"
                                                            onClick={() => setDeleteTarget(exam)}
                                                            className="text-xs font-bold text-red-600 hover:underline"
                                                        >
                                                            Delete
                                                        </button>
                                                    </div>
                                                </td>
                                            </tr>
                                        ))
                                    )}
                                </tbody>
                            </table>
                        </div>
                    </div>

                    <DeleteConfirmModal
                        open={!!deleteTarget}
                        title={deleteTarget?.bulk ? 'Delete selected exams?' : 'Delete exam?'}
                        message={
                            deleteTarget?.bulk
                                ? `This will permanently delete ${selected.size} exams.`
                                : `Delete "${deleteTarget?.title}"? This cannot be undone.`
                        }
                        loading={deleting}
                        onCancel={() => setDeleteTarget(null)}
                        onConfirm={handleDelete}
                    />
                </>
            )}
        </AdminLayout>
    );
}
