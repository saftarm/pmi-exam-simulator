import type { QuestionFormAnswerOption, QuestionFormState, QuestionType } from '../types';

export function updateAnswerOption(
  options: QuestionFormAnswerOption[],
  index: number,
  field: keyof QuestionFormAnswerOption,
  value: string | boolean,
) {
  const next = [...options];
  next[index] = { ...next[index], [field]: value };
  return next;
}

export function addAnswerOption(options: QuestionFormAnswerOption[]) {
  return [...options, { text: '', isCorrect: false }];
}

export function removeAnswerOption(options: QuestionFormAnswerOption[], index: number) {
  return options.filter((_, i) => i !== index);
}

export function applyQuestionTypeChange(currentForm: QuestionFormState, value: QuestionType): QuestionFormState {
  if (value === 'TrueFalse') {
    return {
      ...currentForm,
      questionType: value,
      answerOptions: [
        { text: 'True', isCorrect: false },
        { text: 'False', isCorrect: false },
      ],
    };
  }
  return { ...currentForm, questionType: value };
}
