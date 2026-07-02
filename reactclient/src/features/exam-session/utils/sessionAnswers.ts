import { isAxiosError } from 'axios';
import { formatApiErrors } from '../../../shared/api/errors';
import type { ApiId } from '../../../shared/api/primitives';
import type { QuestionSnapshotDto } from '../types';

export type SessionAnswerValue = ApiId | ApiId[] | null | undefined;
export type SessionAnswersMap = Record<number, SessionAnswerValue>;

export function getSessionErrorMessage(error: unknown, context = 'start') {
  const status = isAxiosError(error) ? error.response?.status : undefined;
  if (status === 409) {
    return 'You already have an active session for this exam. Finish or abandon it before starting again.';
  }
  if (status === 503) {
    return 'Session service is temporarily unavailable. Please try again in a few minutes.';
  }
  if (status === 404) {
    return context === 'finish'
      ? 'Session expired or not found. Please start the exam again from the exam page.'
      : 'Exam session could not be found. Please try starting again.';
  }
  if (status === 422) {
    return 'Your answers could not be validated. Check that every answered question has a selection and try again.';
  }
  const normalizedError = error instanceof Error ? error : new Error(String(error));
  return formatApiErrors(normalizedError) || (context === 'finish' ? 'Failed to submit exam' : 'Could not start exam session');
}

export function buildSessionResponses(answers: SessionAnswersMap, questions: QuestionSnapshotDto[]) {
  return Object.entries(answers).map(([idx, value]) => {
    const questionId = questions[Number(idx)].questionId;
    const selectedOptionIds = Array.isArray(value) ? value : value != null ? [value] : [];
    return { questionId, selectedOptionIds };
  });
}

function hasAnswer(value: SessionAnswerValue) {
  if (value == null) return false;
  if (Array.isArray(value)) return value.length > 0;
  return true;
}

export function countAnswered(answers: SessionAnswersMap) {
  return Object.values(answers).filter(hasAnswer).length;
}

export function isOptionSelected(answerValue: SessionAnswerValue, optionId: ApiId) {
  if (Array.isArray(answerValue)) return answerValue.includes(optionId);
  return answerValue === optionId;
}
