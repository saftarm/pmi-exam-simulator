export function updateAnswerOption(options, index, field, value) {
  const next = [...options];
  next[index] = { ...next[index], [field]: value };
  return next;
}

export function addAnswerOption(options) {
  return [...options, { text: '', isCorrect: false }];
}

export function removeAnswerOption(options, index) {
  return options.filter((_, i) => i !== index);
}

export function applyQuestionTypeChange(currentForm, value) {
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
