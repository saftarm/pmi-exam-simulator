import { useEffect, useMemo, useState, type FormEvent } from 'react';
import { useNavigate } from 'react-router-dom';
import { isAxiosError } from 'axios';
import { AdminLayout } from '../../auth';
import QuestionForm from '../components/QuestionForm';
import BackLink from '../../../shared/components/BackLink';
import ErrorBanner from '../../../shared/components/ErrorBanner';
import { getAllDomains } from '../../admin-categories/api';
import { getAllExams } from '../../admin-exams/api';
import { createQuestion } from '../api';
import { formatApiErrors } from '../../../shared/api/errors';
import type { ApiId } from '../../../shared/api/primitives';
import type { DomainDto } from '../../admin-categories/types';
import type { ExamSummaryDto } from '../../admin-exams/types';
import type { QuestionFormState } from '../types';
import { FormSkeleton } from '../../../shared/components/loading';

export default function AdminQuestionCreatePage() {
  const navigate = useNavigate();
  const [domains, setDomains] = useState<DomainDto[]>([]);
  const [exams, setExams] = useState<ExamSummaryDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [form, setForm] = useState<QuestionFormState>({
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
    () => Object.fromEntries(exams.map((e) => [e.id, e.title])) as Record<ApiId, string>,
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

  const handleSave = async (e: FormEvent<HTMLFormElement>) => {
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
    } catch (err: unknown) {
      const normalizedError = err instanceof Error ? err : new Error(String(err));
      setError(formatApiErrors(isAxiosError(err) ? err : normalizedError));
    } finally {
      setSaving(false);
    }
  };

  const domainLabel = (d: DomainDto) => {
    const examTitle = examTitleById[d.examId];
    return examTitle ? `${d.title} (exam: ${examTitle})` : (d.title ?? '');
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
