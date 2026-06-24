import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import AdminLayout from '../../components/admin/AdminLayout';
import Icon from '../../components/Icon';
import DeleteConfirmModal from '../../components/admin/DeleteConfirmModal';
import { getQuestion, updateQuestion, deleteQuestion } from '../../services/adminQuestionService';
import { formatApiErrors } from '../../services/authService';
import { QUESTION_TYPES, normalizeQuestionType } from '../../utils/questionTypes';
import { FormSkeleton, LoadingButton } from '../../components/loading';

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
    questionType: 'SingleChoice',
    answerOptions: [],
  });
  const [domainMeta, setDomainMeta] = useState({ domainTitle: '', examTitle: '' });

  useEffect(() => {
    getQuestion(questionId)
      .then((q) => {
        setDomainMeta({
          domainTitle: q.domainTitle || '',
          examTitle: q.examTitle || '',
        });
        setForm({
          title: q.title || '',
          explanation: q.explanation || '',
          questionType: normalizeQuestionType(q.questionType),
          answerOptions: (q.answerOptions || []).map((o) => ({
            id: o.id,
            text: o.text || '',
            isCorrect: !!o.isCorrect,
          })),
        });
      })
      .catch(() => setError('Question not found'))
      .finally(() => setLoading(false));
  }, [questionId]);

  const updateOption = (index, field, value) => {
    setForm((f) => {
      const options = [...f.answerOptions];
      options[index] = { ...options[index], [field]: value };
      return { ...f, answerOptions: options };
    });
  };

  const addOption = () => {
    setForm((f) => ({
      ...f,
      answerOptions: [...f.answerOptions, { text: '', isCorrect: false }],
    }));
  };

  const removeOption = (index) => {
    setForm((f) => ({
      ...f,
      answerOptions: f.answerOptions.filter((_, i) => i !== index),
    }));
  };

  const handleSave = async (e) => {
    e.preventDefault();
    if (!form.answerOptions.some((o) => o.isCorrect)) {
      setError('Mark at least one answer option as correct.');
      return;
    }
    setSaving(true);
    setError(null);
    try {
      await updateQuestion(questionId, {
        title: form.title,
        explanation: form.explanation,
        questionType: form.questionType,
        answerOptionsDtos: form.answerOptions.map((o) => ({
          id: o.id || undefined,
          text: o.text,
          isCorrect: !!o.isCorrect,
        })),
      });
      navigate('/admin/questions');
    } catch (err) {
      setError(formatApiErrors(err));
    } finally {
      setSaving(false);
    }
  };

  const handleDelete = async () => {
    setDeleting(true);
    try {
      await deleteQuestion(questionId);
      navigate('/admin/questions');
    } catch (err) {
      setError(formatApiErrors(err));
      setShowDelete(false);
    } finally {
      setDeleting(false);
    }
  };

  if (loading) {
    return (
      <AdminLayout title="Edit Question">
        <FormSkeleton fields={6} />
      </AdminLayout>
    );
  }

  return (
    <AdminLayout title="Edit Question">
      <button
        type="button"
        onClick={() => navigate('/admin/questions')}
        className="mb-lg text-sm text-secondary-container font-bold hover:underline flex items-center gap-xs"
      >
        <Icon name="arrow_back" style={{ fontSize: 18 }} />
        Back
      </button>

      {error && (
        <div className="mb-lg p-md bg-red-50 text-red-700 rounded-lg border border-red-200">
          {error}
        </div>
      )}

      <form
        onSubmit={handleSave}
        className="bg-white rounded-xl border border-outline-variant shadow-sm p-lg max-w-2xl space-y-md"
      >
        <p className="text-xs text-on-surface-variant font-mono">ID: {questionId}</p>
        {(domainMeta.domainTitle || domainMeta.examTitle) && (
          <div className="text-sm text-on-surface-variant bg-surface-container-low rounded-lg p-md">
            <p>
              <span className="font-bold">Domain:</span> {domainMeta.domainTitle || '—'}
            </p>
            {domainMeta.examTitle && (
              <p className="mt-xs">
                <span className="font-bold">Exam context:</span> {domainMeta.examTitle}
              </p>
            )}
          </div>
        )}

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

        <div>
          <div className="flex justify-between items-center mb-sm">
            <label className="block text-sm font-bold">Answer options</label>
            <button
              type="button"
              onClick={addOption}
              className="text-sm font-bold text-secondary-container hover:underline"
            >
              + Add option
            </button>
          </div>
          <div className="space-y-sm">
            {form.answerOptions.map((opt, index) => (
              <div
                key={opt.id || `new-${index}`}
                className="flex flex-col sm:flex-row gap-sm border border-outline-variant rounded-lg p-md"
              >
                <input
                  value={opt.text}
                  onChange={(e) => updateOption(index, 'text', e.target.value)}
                  placeholder="Answer text"
                  className="flex-1 border border-outline-variant rounded-lg px-md py-sm text-sm"
                />
                <label className="flex items-center gap-sm text-sm shrink-0">
                  <input
                    type="checkbox"
                    checked={opt.isCorrect}
                    onChange={(e) => updateOption(index, 'isCorrect', e.target.checked)}
                  />
                  Correct
                </label>
                <button
                  type="button"
                  onClick={() => removeOption(index)}
                  className="text-xs font-bold text-red-600"
                >
                  Remove
                </button>
              </div>
            ))}
          </div>
        </div>

        <div className="flex gap-md pt-md border-t border-outline-variant">
          <LoadingButton
            type="submit"
            loading={saving}
            loadingText="Saving…"
            className="bg-secondary-container text-white px-lg py-sm rounded-lg font-bold disabled:opacity-50"
          >
            Save Question
          </LoadingButton>
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
