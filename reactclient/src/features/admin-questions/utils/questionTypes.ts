import type { QuestionType } from '../types';

export const QUESTION_TYPES = [
  { value: 'SingleChoice', label: 'Single Choice' },
  { value: 'MultipleChoice', label: 'Multiple Choice' },
  { value: 'TrueFalse', label: 'True / False' },
] as const satisfies ReadonlyArray<{ value: QuestionType; label: string }>;

export function normalizeQuestionType(value: unknown): QuestionType {
  if (typeof value === 'string') return value as QuestionType;
  const map: Record<number, QuestionType> = {
    1: 'SingleChoice',
    2: 'MultipleChoice',
    3: 'TrueFalse',
  };
  return map[value as number] ?? 'SingleChoice';
}

export function questionTypeLabel(value: unknown) {
  const normalized = normalizeQuestionType(value);
  return QUESTION_TYPES.find((t) => t.value === normalized)?.label ?? normalized;
}
