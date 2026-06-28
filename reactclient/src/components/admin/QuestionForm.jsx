import { QUESTION_TYPES } from '../../utils/questionTypes';
import { applyQuestionTypeChange } from '../../utils/questionFormState';
import AnswerOptionsEditor from './AnswerOptionsEditor';
import { LoadingButton } from '../loading';

export default function QuestionForm({
  mode,
  form,
  onChange,
  onSubmit,
  domains = [],
  domainLabel,
  domainMeta,
  questionId,
  saving,
  submitLabel,
  submitLoadingText,
  disabled = false,
  children,
}) {
  const handleQuestionTypeChange = (value) => {
    onChange(applyQuestionTypeChange(form, value));
  };

  return (
    <form
      onSubmit={onSubmit}
      className="bg-white rounded-xl border border-outline-variant shadow-sm p-lg max-w-2xl space-y-md"
    >
      {mode === 'edit' && questionId && (
        <p className="text-xs text-on-surface-variant font-mono">ID: {questionId}</p>
      )}

      {mode === 'edit' && domainMeta && (domainMeta.domainTitle || domainMeta.examTitle) && (
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

      {mode === 'create' && (
        <div>
          <label className="block text-sm font-bold mb-sm">Domain</label>
          <select
            required
            value={form.domainId}
            onChange={(e) => onChange({ ...form, domainId: e.target.value })}
            className="w-full border border-outline-variant rounded-lg px-md py-sm"
            disabled={domains.length === 0}
          >
            {domains.map((d) => (
              <option key={d.id} value={d.id}>
                {domainLabel ? domainLabel(d) : d.title}
              </option>
            ))}
          </select>
        </div>
      )}

      <div>
        <label className="block text-sm font-bold mb-sm">Question text</label>
        <textarea
          required
          maxLength={mode === 'create' ? 1000 : undefined}
          rows={4}
          value={form.title}
          onChange={(e) => onChange({ ...form, title: e.target.value })}
          className="w-full border border-outline-variant rounded-lg px-md py-sm"
        />
      </div>

      <div>
        <label className="block text-sm font-bold mb-sm">Explanation</label>
        <textarea
          maxLength={mode === 'create' ? 1000 : undefined}
          rows={3}
          value={form.explanation}
          onChange={(e) => onChange({ ...form, explanation: e.target.value })}
          className="w-full border border-outline-variant rounded-lg px-md py-sm"
        />
      </div>

      <div>
        <label className="block text-sm font-bold mb-sm">Question type</label>
        <select
          value={form.questionType}
          onChange={(e) =>
            mode === 'create'
              ? handleQuestionTypeChange(e.target.value)
              : onChange({ ...form, questionType: e.target.value })
          }
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

      <AnswerOptionsEditor
        options={form.answerOptions}
        onChange={(answerOptions) => onChange({ ...form, answerOptions })}
      />

      {children || (
        <LoadingButton
          type="submit"
          loading={saving}
          loadingText={submitLoadingText}
          disabled={disabled}
          className="bg-secondary-container text-white px-lg py-sm rounded-lg font-bold disabled:opacity-50"
        >
          {submitLabel}
        </LoadingButton>
      )}
    </form>
  );
}
