import { formatApiErrors } from '../services/authService';

export function getSessionErrorMessage(error, context = 'start') {
  const status = error.response?.status;
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
  return formatApiErrors(error) || (context === 'finish' ? 'Failed to submit exam' : 'Could not start exam session');
}

export function buildSessionResponses(answers, questions) {
  return Object.entries(answers).map(([idx, value]) => {
    const questionId = questions[Number(idx)].questionId;
    const selectedOptionIds = Array.isArray(value) ? value : [value];
    return { questionId, selectedOptionIds };
  });
}

function hasAnswer(value) {
  if (value == null) return false;
  if (Array.isArray(value)) return value.length > 0;
  return true;
}

export function countAnswered(answers) {
  return Object.values(answers).filter(hasAnswer).length;
}

export function isOptionSelected(answerValue, optionId) {
  if (Array.isArray(answerValue)) return answerValue.includes(optionId);
  return answerValue === optionId;
}
