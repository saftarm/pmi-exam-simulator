import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import AdminLayout from '../../components/admin/AdminLayout';
import QuestionForm from '../../components/admin/QuestionForm';
import DeleteConfirmModal from '../../components/admin/DeleteConfirmModal';
import BackLink from '../../components/BackLink';
import ErrorBanner from '../../components/ErrorBanner';
import { getQuestion, updateQuestion, deleteQuestion } from '../../services/adminQuestionService';
import { formatApiErrors } from '../../services/authService';
import { normalizeQuestionType } from '../../utils/questionTypes';
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
      <BackLink onClick={() => navigate('/admin/questions')} label="Back" />
      <ErrorBanner message={error} />

      <QuestionForm
        mode="edit"
        form={form}
        onChange={setForm}
        onSubmit={handleSave}
        domainMeta={domainMeta}
        questionId={questionId}
        saving={saving}
        submitLabel="Save Question"
        submitLoadingText="Saving…"
      >
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
      </QuestionForm>

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
