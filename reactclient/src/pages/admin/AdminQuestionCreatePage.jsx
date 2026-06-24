import { useEffect, useMemo, useState } from 'react';

import { Link, useNavigate } from 'react-router-dom';

import AdminLayout from '../../components/admin/AdminLayout';

import Icon from '../../components/Icon';

import { getAllDomains } from '../../services/adminDomainService';

import { getAllExams } from '../../services/adminExamService';

import { createQuestion } from '../../services/adminQuestionService';

import { formatApiErrors } from '../../services/authService';

import { QUESTION_TYPES } from '../../utils/questionTypes';

import { FormSkeleton, LoadingButton } from '../../components/loading';

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

  const handleQuestionTypeChange = (value) => {
    setForm((f) => {
      if (value === 'TrueFalse') {
        return {
          ...f,
          questionType: value,
          answerOptions: [
            { text: 'True', isCorrect: false },
            { text: 'False', isCorrect: false },
          ],
        };
      }
      return { ...f, questionType: value };
    });
  };

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
      <Link
        to="/admin/questions"
        className="mb-lg text-sm text-secondary-container font-bold hover:underline flex items-center gap-xs"
      >
        <Icon name="arrow_back" style={{ fontSize: 18 }} />
        Back to Question Pool
      </Link>

      {error && (
        <div className="mb-lg p-md bg-red-50 text-red-700 rounded-lg border border-red-200">
          {error}
        </div>
      )}

      {domains.length === 0 && (
        <div className="mb-lg p-md bg-amber-50 text-amber-800 rounded-lg border border-amber-200 text-sm">
          No domains exist yet. Create an exam with domains before adding questions.
        </div>
      )}

      <form
        onSubmit={handleSave}
        className="bg-white rounded-xl border border-outline-variant shadow-sm p-lg max-w-2xl space-y-md"
      >
        <div>
          <label className="block text-sm font-bold mb-sm">Domain</label>

          <select
            required
            value={form.domainId}
            onChange={(e) => setForm({ ...form, domainId: e.target.value })}
            className="w-full border border-outline-variant rounded-lg px-md py-sm"
            disabled={domains.length === 0}
          >
            {domains.map((d) => (
              <option key={d.id} value={d.id}>
                {domainLabel(d)}
              </option>
            ))}
          </select>
        </div>

        <div>
          <label className="block text-sm font-bold mb-sm">Question text</label>

          <textarea
            required
            maxLength={1000}
            rows={4}
            value={form.title}
            onChange={(e) => setForm({ ...form, title: e.target.value })}
            className="w-full border border-outline-variant rounded-lg px-md py-sm"
          />
        </div>

        <div>
          <label className="block text-sm font-bold mb-sm">Explanation</label>

          <textarea
            maxLength={1000}
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
            onChange={(e) => handleQuestionTypeChange(e.target.value)}
            className="w-full border border-outline-variant rounded-lg px-md py-sm"
          >
            {QUESTION_TYPES.map((t) => (
              <option key={t.value} value={t.value}>
                {t.label}
              </option>
            ))}
          </select>
          {form.questionType === 'MultipleChoice' && (
            <p className="text-sm text-on-surface-variant mt-sm">
              Mark every correct option. Learners can select multiple answers; partial credit applies
              in exam sessions.
            </p>
          )}
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
                key={`new-${index}`}
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

        <LoadingButton
          type="submit"
          loading={saving}
          loadingText="Creating…"
          disabled={domains.length === 0}
          className="bg-secondary-container text-white px-lg py-sm rounded-lg font-bold disabled:opacity-50"
        >
          Create Question
        </LoadingButton>
      </form>
    </AdminLayout>
  );
}
