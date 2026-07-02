import type { ApiId } from '../../../shared/api/primitives';
import type { QuestionSnapshotDto } from '../types';

const SESSION_QUESTIONS_PREFIX = 'exam_session_questions_';

export function saveSessionQuestions(sessionId: ApiId, questions: QuestionSnapshotDto[]) {
  if (!sessionId) return;
  localStorage.setItem(`${SESSION_QUESTIONS_PREFIX}${sessionId}`, JSON.stringify(questions ?? []));
}

export function loadSessionQuestions(sessionId: ApiId): QuestionSnapshotDto[] | null {
  if (!sessionId) return null;
  try {
    const raw = localStorage.getItem(`${SESSION_QUESTIONS_PREFIX}${sessionId}`);
    if (!raw) return null;
    const parsed: unknown = JSON.parse(raw);
    return Array.isArray(parsed) ? (parsed as QuestionSnapshotDto[]) : null;
  } catch {
    return null;
  }
}

export function clearSessionQuestions(sessionId: ApiId) {
  if (!sessionId) return;
  localStorage.removeItem(`${SESSION_QUESTIONS_PREFIX}${sessionId}`);
}
