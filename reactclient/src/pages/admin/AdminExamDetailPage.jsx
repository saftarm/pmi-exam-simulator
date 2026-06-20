import { useEffect, useState } from 'react';
import { Link, useNavigate, useParams } from 'react-router-dom';
import AdminLayout from '../../components/admin/AdminLayout';
import StatusBadge from '../../components/admin/StatusBadge';
import Icon from '../../components/Icon';
import { getExamDetails, publishExam, getAllExams } from '../../services/adminExamService';
import { getDomainsByExam } from '../../services/adminDomainService';
import { importQuestions } from '../../services/adminQuestionService';
import { logActivity } from '../../services/adminMockStore';
import { formatExamStatus, statusBadgeType } from '../../utils/examStatus';

export default function AdminExamDetailPage() {
    const { examId } = useParams();
    const navigate = useNavigate();
    const [exam, setExam] = useState(null);
    const [examMeta, setExamMeta] = useState(null);
    const [domains, setDomains] = useState([]);
    const [loading, setLoading] = useState(true);
    const [importing, setImporting] = useState(false);
    const [importResult, setImportResult] = useState(null);
    const [publishing, setPublishing] = useState(false);

    useEffect(() => {
        let cancelled = false;

        async function load() {
            try {
                const [details, domainList, allExams] = await Promise.all([
                    getExamDetails(examId),
                    getDomainsByExam(examId),
                    getAllExams(),
                ]);
                if (!cancelled) {
                    setExam(details);
                    setDomains(domainList || []);
                    setExamMeta(allExams.find((e) => e.id === examId) || null);
                }
            } catch {
                if (!cancelled) setExam(null);
            } finally {
                if (!cancelled) setLoading(false);
            }
        }

        load();
        return () => { cancelled = true; };
    }, [examId]);

    const handleImport = async (e) => {
        const file = e.target.files?.[0];
        if (!file) return;
        setImporting(true);
        setImportResult(null);
        try {
            const result = await importQuestions(examId, file);
            setImportResult({ success: true, message: `Imported ${result?.length ?? 'questions'} successfully.` });
            logActivity({
                user: 'Admin',
                initials: 'AD',
                action: `Imported questions into "${exam?.title}"`,
                status: 'Success',
                statusType: 'success',
            });
        } catch (err) {
            setImportResult({
                success: false,
                message: err.response?.data?.title || err.message || 'Import failed',
            });
        } finally {
            setImporting(false);
            e.target.value = '';
        }
    };

    const handlePublish = async () => {
        setPublishing(true);
        try {
            await publishExam(examId);
            logActivity({
                user: 'Admin',
                initials: 'AD',
                action: `Published exam "${exam?.title}"`,
                status: 'Published',
                statusType: 'success',
            });
            const allExams = await getAllExams();
            setExamMeta(allExams.find((e) => e.id === examId) || null);
        } catch (err) {
            alert(err.response?.data?.title || err.message || 'Publish failed');
        } finally {
            setPublishing(false);
        }
    };

    if (loading) {
        return (
            <AdminLayout title="Exam Details">
                <p className="text-on-surface-variant">Loading…</p>
            </AdminLayout>
        );
    }

    if (!exam) {
        return (
            <AdminLayout title="Exam Details">
                <p className="text-red-600">Exam not found.</p>
                <Link to="/admin/exams" className="text-secondary-container font-bold hover:underline">
                    Back to exams
                </Link>
            </AdminLayout>
        );
    }

    const status = examMeta?.status;

    return (
        <AdminLayout title={exam.title || 'Exam Details'}>
            <button
                type="button"
                onClick={() => navigate('/admin/exams')}
                className="mb-lg text-sm text-secondary-container font-bold hover:underline flex items-center gap-xs"
            >
                <Icon name="arrow_back" style={{ fontSize: 18 }} />
                Back to exams
            </button>

            <div className="grid grid-cols-1 lg:grid-cols-3 gap-lg">
                <div className="lg:col-span-2 space-y-lg">
                    <div className="bg-white rounded-xl border border-outline-variant shadow-sm p-lg">
                        <div className="flex flex-wrap items-center gap-md mb-md">
                            {status !== undefined && (
                                <StatusBadge status={formatExamStatus(status)} type={statusBadgeType(status)} />
                            )}
                        </div>
                        <p className="text-on-surface-variant mb-lg">{exam.context || 'No description.'}</p>
                        <dl className="grid grid-cols-2 gap-md text-sm">
                            <div>
                                <dt className="text-on-surface-variant">Duration</dt>
                                <dd className="font-bold">{exam.durationInMinutes} minutes</dd>
                            </div>
                            <div>
                                <dt className="text-on-surface-variant">Questions</dt>
                                <dd className="font-bold">{exam.numberOfQuestions}</dd>
                            </div>
                        </dl>
                        <div className="flex flex-wrap gap-md mt-lg pt-lg border-t border-outline-variant">
                            <Link
                                to={`/admin/exams/${examId}/edit`}
                                className="px-md py-sm rounded-lg border border-outline-variant font-bold text-sm hover:bg-surface-container-low"
                            >
                                Edit duration & questions
                            </Link>
                            {formatExamStatus(status) !== 'Published' && (
                                <button
                                    type="button"
                                    onClick={handlePublish}
                                    disabled={publishing}
                                    className="px-md py-sm rounded-lg bg-green-600 text-white font-bold text-sm disabled:opacity-50"
                                >
                                    {publishing ? 'Publishing…' : 'Publish Exam'}
                                </button>
                            )}
                        </div>
                    </div>

                    <div className="bg-white rounded-xl border border-outline-variant shadow-sm p-lg">
                        <h2 className="font-headline-sm text-headline-sm font-bold mb-md">Domains</h2>
                        {domains.length === 0 ? (
                            <p className="text-on-surface-variant text-sm">No domains found.</p>
                        ) : (
                            <ul className="divide-y divide-outline-variant">
                                {domains.map((d) => (
                                    <li key={d.id} className="py-md flex justify-between">
                                        <div>
                                            <p className="font-medium">{d.title}</p>
                                            <p className="text-sm text-on-surface-variant">{d.description}</p>
                                        </div>
                                        <span className="text-sm text-on-surface-variant">Weight: {d.weight}</span>
                                    </li>
                                ))}
                            </ul>
                        )}
                    </div>
                </div>

                <div className="space-y-lg">
                    <div className="bg-white rounded-xl border border-outline-variant shadow-sm p-lg">
                        <h2 className="font-headline-sm text-headline-sm font-bold mb-md">Import Questions</h2>
                        <p className="text-sm text-on-surface-variant mb-md">
                            Upload an Excel file (.xlsx) with questions for this exam.
                        </p>
                        <label className="flex flex-col items-center justify-center border-2 border-dashed border-outline-variant rounded-lg p-lg cursor-pointer hover:bg-surface-container-low transition-colors">
                            <Icon name="upload_file" className="text-secondary-container mb-sm" />
                            <span className="text-sm font-bold">
                                {importing ? 'Importing…' : 'Choose Excel file'}
                            </span>
                            <input
                                type="file"
                                accept=".xlsx,.xls"
                                className="hidden"
                                disabled={importing}
                                onChange={handleImport}
                            />
                        </label>
                        {importResult && (
                            <p
                                className={`mt-md text-sm font-medium ${
                                    importResult.success ? 'text-green-600' : 'text-red-600'
                                }`}
                            >
                                {importResult.message}
                            </p>
                        )}
                    </div>

                    <div className="bg-white rounded-xl border border-outline-variant shadow-sm p-lg">
                        <h2 className="font-headline-sm text-headline-sm font-bold mb-md">Edit Question by ID</h2>
                        <p className="text-sm text-on-surface-variant mb-md">
                            Enter a question GUID to open the editor.
                        </p>
                        <QuestionIdLookup />
                    </div>
                </div>
            </div>
        </AdminLayout>
    );
}

function QuestionIdLookup() {
    const [questionId, setQuestionId] = useState('');
    const navigate = useNavigate();

    return (
        <form
            onSubmit={(e) => {
                e.preventDefault();
                if (questionId.trim()) navigate(`/admin/questions/${questionId.trim()}`);
            }}
            className="flex gap-sm"
        >
            <input
                value={questionId}
                onChange={(e) => setQuestionId(e.target.value)}
                placeholder="Question GUID"
                className="flex-1 border border-outline-variant rounded-lg px-md py-sm text-sm"
            />
            <button
                type="submit"
                className="px-md py-sm bg-secondary-container text-white rounded-lg text-sm font-bold"
            >
                Open
            </button>
        </form>
    );
}
