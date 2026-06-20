const STATUS_LABELS = ['Draft', 'Compiled', 'Published', 'Archived'];

export function formatExamStatus(status) {
    if (typeof status === 'string') return status;
    return STATUS_LABELS[status] ?? 'Draft';
}

export function isPublishedExam(exam) {
    return exam.status === 'Published' || exam.status === 2;
}

export function statusBadgeType(status) {
    const label = formatExamStatus(status);
    if (label === 'Published') return 'success';
    if (label === 'Draft') return 'neutral';
    if (label === 'Compiled') return 'info';
    return 'warning';
}
