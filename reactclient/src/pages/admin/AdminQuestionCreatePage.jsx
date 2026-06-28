import { useEffect, useMemo, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import AdminLayout from '../../components/admin/AdminLayout';
import QuestionForm from '../../components/admin/QuestionForm';
import BackLink from '../../components/BackLink';
import ErrorBanner from '../../components/ErrorBanner';
import { getAllDomains } from '../../services/adminDomainService';
import { getAllExams } from '../../services/adminExamService';
import { createQuestion } from '../../services/adminQuestionService';
import { formatApiErrors } from '../../services/authService';
import { FormSkeleton } from '../../components/loading';

export default function AdminQuestionCreatePage() {
  const navigate = useNavigate();
  const [domains, setDomains] = useState([]);
  const [exams, setExams] = useState([]);
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState(null);
  const [form, setForm] = useState({
    domainId: '',
    title: '',
    explanation: '',
    questionType: 'SingleChoice',
    answerOptions: [
      { text: '', isCorrect: false },
      { text: '', isCorrect: false },
    ],
  });

  const examTitleById = useMemo(
    () => Object.fromEntries(exams.map((e) => [e.id, e.title])),
    [exams],
  );

  useEffect(() => {
    Promise.all([getAllDomains(), getAllExams()])
      .then(([domainList, examList]) => {
        setDomains(domainList);
        setExams(examList);
        if (domainList.length > 0) {
          setForm((f) => ({ ...f, domainId: domainList[0].id }));
        }
      })
      .catch(() => {
        setDomains([]);
        setExams([]);
      })
      .finally(() => setLoading(false));
  }, []);

  const handleSave = async (e) => {
    e.preventDefault();
    if (!form.domainId) {
      setError('Select a domain for this question.');
      return;
    }
    if (!form.answerOptions.some((o) => o.isCorrect)) {
      setError('Mark at least one answer option as correct.');
      return;
    }
    setSaving(true);
    setError(null);
    try {
      await createQuestion({
        domainId: form.domainId,
        title: form.title,
        explanation: form.explanation,
        questionType: form.questionType,
        answerOptionsDtos: form.answerOptions.map((o) => ({
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

  const domainLabel = (d) => {
    const examTitle = examTitleById[d.examId];
    return examTitle ? `${d.title} (exam: ${examTitle})` : d.title;
  };

  if (loading) {
    return (
      <AdminLayout title="Add Question">
        <FormSkeleton fields={6} />
      </AdminLayout>
    );
  }

  return (
    <AdminLayout title="Add Question">
      <BackLink to="/admin/questions" label="Back to Question Pool" />
      <ErrorBanner message={error} />

      {domains.length === 0 && (
        <div className="mb-lg p-md bg-amber-50 text-amber-800 rounded-lg border border-amber-200 text-sm">
          No domains exist yet. Create an exam with domains before adding questions.
        </div>
      )}

      <QuestionForm
        mode="create"
        form={form}
        onChange={setForm}
        onSubmit={handleSave}
        domains={domains}
        domainLabel={domainLabel}
        saving={saving}
        submitLabel="Create Question"
        submitLoadingText="Creating…"
        disabled={domains.length === 0}
      />
    </AdminLayout>
  );
}
