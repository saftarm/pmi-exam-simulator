export const QUESTION_TYPES = [
    { value: 'SingleChoice', label: 'Single Choice' },
    { value: 'MultipleChoice', label: 'Multiple Choice' },
    { value: 'TrueFalse', label: 'True / False' },
];

export function normalizeQuestionType(value) {
    if (typeof value === 'string') return value;
    const map = { 1: 'SingleChoice', 2: 'MultipleChoice', 3: 'TrueFalse' };
    return map[value] ?? 'SingleChoice';
}

export function questionTypeLabel(value) {
    const normalized = normalizeQuestionType(value);
    return QUESTION_TYPES.find((t) => t.value === normalized)?.label ?? normalized;
}
