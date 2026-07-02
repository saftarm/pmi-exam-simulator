const STATUS_LABELS: Record<number, string> = { 0: 'Draft', 2: 'Published', 3: 'Archived' };

export function formatExamStatus(status: string | number | null | undefined) {
  if (typeof status === 'string') {
    if (status === 'Compiled') return 'Draft';
    return status;
  }
  return STATUS_LABELS[status ?? 0] ?? 'Draft';
}

export function isPublishedExam(exam: { status?: string | number | null }) {
  return exam.status === 'Published' || exam.status === 2;
}

export function statusBadgeType(status: string | number | null | undefined) {
  const label = formatExamStatus(status);
  if (label === 'Published') return 'success';
  if (label === 'Draft') return 'neutral';
  return 'warning';
}

export function canArchiveExam(exam: { status?: string | number | null }) {
  return formatExamStatus(exam.status) === 'Published';
}