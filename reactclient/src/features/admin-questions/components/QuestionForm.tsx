import type { ChangeEvent, FormEvent, ReactNode } from 'react';
import { QUESTION_TYPES } from '../utils/questionTypes';
import { applyQuestionTypeChange } from '../utils/questionFormState';
import AnswerOptionsEditor from './AnswerOptionsEditor';
import { LoadingButton } from '../../../shared/components/loading';
import type { DomainDto } from '../../admin-categories/types';
import type { QuestionFormAnswerOption, QuestionFormState, QuestionType } from '../types';

interface QuestionFormBaseProps {
  form: QuestionFormState;
  onChange: (form: QuestionFormState) => void;
  onSubmit: (e: FormEvent<HTMLFormElement>) => void;
  saving: boolean;
  submitLabel: string;
  submitLoadingText: string;
  disabled?: boolean;
  children?: ReactNode;
}

interface QuestionFormCreateProps extends QuestionFormBaseProps {
  mode: 'create';
  domains: DomainDto[];
  domainLabel?: (domain: DomainDto) => string;
}

interface QuestionFormEditProps extends QuestionFormBaseProps {
  mode: 'edit';
  questionId: string;
  domainMeta?: { domainTitle: string; examTitle: string };
}

type QuestionFormProps = QuestionFormCreateProps | QuestionFormEditProps;

export default function QuestionForm(props: QuestionFormProps) {
  const {
    mode,
    form,
    onChange,
    onSubmit,
    saving,
    submitLabel,
    submitLoadingText,
    disabled = false,
    children,
  } = props;

  const handleQuestionTypeChange = (value: QuestionType) => {
    onChange(applyQuestionTypeChange(form, value));
  };

  return (
    <form
      onSubmit={onSubmit}
      className="bg-white rounded-xl border border-outline-variant shadow-sm p-lg max-w-2xl space-y-md"
    >
      {mode === 'edit' && props.questionId && (
        <p className="text-xs text-on-surface-variant font-mono">ID: {props.questionId}</p>
      )}

      {mode === 'edit' &&
        props.domainMeta &&
        (props.domainMeta.domainTitle || props.domainMeta.examTitle) && (
          <div className="text-sm text-on-surface-variant bg-surface-container-low rounded-lg p-md">
            <p>
              <span className="font-bold">Domain:</span> {props.domainMeta.domainTitle || '—'}
            </p>
            {props.domainMeta.examTitle && (
              <p className="mt-xs">
                <span className="font-bold">Exam context:</span> {props.domainMeta.examTitle}
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
            onChange={(e: ChangeEvent<HTMLSelectElement>) =>
              onChange({ ...form, domainId: e.target.value })
            }
            className="w-full border border-outline-variant rounded-lg px-md py-sm"
            disabled={props.domains.length === 0}
          >
            {props.domains.map((d) => (
              <option key={d.id} value={d.id}>
                {props.domainLabel ? props.domainLabel(d) : d.title}
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
          onChange={(e: ChangeEvent<HTMLTextAreaElement>) =>
            onChange({ ...form, title: e.target.value })
          }
          className="w-full border border-outline-variant rounded-lg px-md py-sm"
        />
      </div>

      <div>
        <label className="block text-sm font-bold mb-sm">Explanation</label>
        <textarea
          maxLength={mode === 'create' ? 1000 : undefined}
          rows={3}
          value={form.explanation}
          onChange={(e: ChangeEvent<HTMLTextAreaElement>) =>
            onChange({ ...form, explanation: e.target.value })
          }
          className="w-full border border-outline-variant rounded-lg px-md py-sm"
        />
      </div>

      <div>
        <label className="block text-sm font-bold mb-sm">Question type</label>
        <select
          value={form.questionType}
          onChange={(e: ChangeEvent<HTMLSelectElement>) =>
            mode === 'create'
              ? handleQuestionTypeChange(e.target.value as QuestionType)
              : onChange({ ...form, questionType: e.target.value as QuestionType })
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
        onChange={(answerOptions: QuestionFormAnswerOption[]) =>
          onChange({ ...form, answerOptions })
        }
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
