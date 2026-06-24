const SESSION_QUESTIONS_PREFIX = 'exam_session_questions_';

export function saveSessionQuestions(sessionId, questions) {
  if (!sessionId) return;
  localStorage.setItem(`${SESSION_QUESTIONS_PREFIX}${sessionId}`, JSON.stringify(questions ?? []));
}

export function loadSessionQuestions(sessionId) {
  if (!sessionId) return null;
  try {
    const raw = localStorage.getItem(`${SESSION_QUESTIONS_PREFIX}${sessionId}`);
    if (!raw) return null;
    const parsed = JSON.parse(raw);
    return Array.isArray(parsed) ? parsed : null;
  } catch {
    return null;
  }
}

export function clearSessionQuestions(sessionId) {
  if (!sessionId) return;
  localStorage.removeItem(`${SESSION_QUESTIONS_PREFIX}${sessionId}`);
}
