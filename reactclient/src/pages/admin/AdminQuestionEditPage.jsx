import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import AdminLayout from '../../components/admin/AdminLayout';
import Icon from '../../components/Icon';
import DeleteConfirmModal from '../../components/admin/DeleteConfirmModal';
import { getQuestion, updateQuestion, deleteQuestion } from '../../services/adminQuestionService';
import { logActivity } from '../../services/adminMockStore';

const QUESTION_TYPES = [
    { value: 1, label: 'Single Choice' },
    { value: 2, label: 'Multiple Choice' },
    { value: 3, label: 'True / False' },
];

export default function AdminQuestionEditPage() {
    const { questionId } = useParams();
    const navigate = useNavigate();
    const [loading, setLoading] = useState(true);
    const [saving, setSaving] = useState(false);
    const [error, setError] = useState(null);
    const [showDelete, setShowDelete] = useState(false);
    const [deleting, setDeleting] = useState(false);
    const [form, setForm] = useState({
        title: '',
        explanation: '',
        questionType: 1,
        answerOptions: [],
    });

    useEffect(() => {
        getQuestion(questionId)
            .then((q) => {
                setForm({
                    title: q.questionTitle || q.title || '',
                    explanation: q.explanation || '',
                    questionType: q.questionType || 1,
                    answerOptions: q.answerOptions || q.answerOptionsDtos || [],
                });
            })
            .catch(() => setError('Question not found'))
            .finally(() => setLoading(false));
    }, [questionId]);

    const handleSave = async (e) => {
        e.preventDefault();
        setSaving(true);
        setError(null);
        try {
            await updateQuestion(questionId, {
                title: form.title,
                explanation: form.explanation,
                questionType: Number(form.questionType),
                answerOptionsDtos: form.answerOptions.map((o) => ({
                    text: o.text,
                    isCorrect: o.isCorrect ?? false,
                })),
            });
            logActivity({
                user: 'Admin',
                initials: 'AD',
                action: `Updated question ${questionId.slice(0, 8)}…`,
                status: 'Updated',
                statusType: 'info',
            });
            navigate(-1);
        } catch (err) {
            setError(err.response?.data?.title || err.message || 'Update failed');
        } finally {
            setSaving(false);
        }
    };

    const handleDelete = async () => {
        setDeleting(true);
        try {
            await deleteQuestion(questionId);
            logActivity({
                user: 'Admin',
                initials: 'AD',
                action: `Deleted question ${questionId.slice(0, 8)}…`,
                status: 'Removed',
                statusType: 'error',
            });
            navigate(-1);
        } catch (err) {
            alert(err.message || 'Delete failed');
        } finally {
            setDeleting(false);
            setShowDelete(false);
        }
    };

    if (loading) {
        return (
            <AdminLayout title="Edit Question">
                <p className="text-on-surface-variant">Loading…</p>
            </AdminLayout>
        );
    }

    return (
        <AdminLayout title="Edit Question">
            <button
                type="button"
                onClick={() => navigate(-1)}
                className="mb-lg text-sm text-secondary-container font-bold hover:underline flex items-center gap-xs"
            >
                <Icon name="arrow_back" style={{ fontSize: 18 }} />
                Back
            </button>

            {error && (
                <div className="mb-lg p-md bg-red-50 text-red-700 rounded-lg border border-red-200">{error}</div>
            )}

            <form onSubmit={handleSave} className="bg-white rounded-xl border border-outline-variant shadow-sm p-lg max-w-2xl space-y-md">
                <p className="text-xs text-on-surface-variant font-mono">ID: {questionId}</p>

                <div>
                    <label className="block text-sm font-bold mb-sm">Question text</label>
                    <textarea
                        required
                        rows={4}
                        value={form.title}
                        onChange={(e) => setForm({ ...form, title: e.target.value })}
                        className="w-full border border-outline-variant rounded-lg px-md py-sm"
                    />
                </div>

                <div>
                    <label className="block text-sm font-bold mb-sm">Explanation</label>
                    <textarea
                        rows={3}
                        value={form.explanation}
                        onChange={(e) => setForm({ ...form, explanation: e.target.value })}
                        className="w-full border border-outline-variant rounded-lg px-md py-sm"
                    />
                </div>

                <div>
                    <label className="block text-sm font-bold mb-sm">Question type</label>
                    <select
                        value={form.questionType}
                        onChange={(e) => setForm({ ...form, questionType: e.target.value })}
                        className="w-full border border-outline-variant rounded-lg px-md py-sm"
                    >
                        {QUESTION_TYPES.map((t) => (
                            <option key={t.value} value={t.value}>
                                {t.label}
                            </option>
                        ))}
                    </select>
                </div>

                {form.answerOptions.length > 0 && (
                    <div>
                        <label className="block text-sm font-bold mb-sm">Answer options (read-only)</label>
                        <ul className="space-y-sm">
                            {form.answerOptions.map((opt, i) => (
                                <li
                                    key={opt.id || i}
                                    className="text-sm border border-outline-variant rounded-lg px-md py-sm text-on-surface-variant"
                                >
                                    {opt.text}
                                </li>
                            ))}
                        </ul>
                    </div>
                )}

                <div className="flex gap-md pt-md border-t border-outline-variant">
                    <button
                        type="submit"
                        disabled={saving}
                        className="bg-secondary-container text-white px-lg py-sm rounded-lg font-bold disabled:opacity-50"
                    >
                        {saving ? 'Saving…' : 'Save Question'}
                    </button>
                    <button
                        type="button"
                        onClick={() => setShowDelete(true)}
                        className="px-lg py-sm rounded-lg border border-red-200 text-red-600 font-bold"
                    >
                        Delete
                    </button>
                </div>
            </form>

            <DeleteConfirmModal
                open={showDelete}
                title="Delete question?"
                message="This will permanently delete the question."
                loading={deleting}
                onCancel={() => setShowDelete(false)}
                onConfirm={handleDelete}
            />
        </AdminLayout>
    );
}
