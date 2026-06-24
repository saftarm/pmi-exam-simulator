const STATUS_LABELS = { 0: 'Draft', 2: 'Published', 3: 'Archived' };

export function formatExamStatus(status) {
  if (typeof status === 'string') {
    if (status === 'Compiled') return 'Draft';
    return status;
  }
  return STATUS_LABELS[status] ?? 'Draft';
}

export function isPublishedExam(exam) {
  return exam.status === 'Published' || exam.status === 2;
}

export function statusBadgeType(status) {
  const label = formatExamStatus(status);
  if (label === 'Published') return 'success';
  if (label === 'Draft') return 'neutral';
  return 'warning';
}

export function canArchiveExam(exam) {
  return formatExamStatus(exam.status) === 'Published';
}
