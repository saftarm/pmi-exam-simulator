import api, { ACCESS_TOKEN_KEY } from './api';

import { saveSessionQuestions } from '../utils/sessionStorage';

export async function getPublishedExams() {
  const { data } = await api.get('/api/exams/details');
  return data;
}

export async function getExamDetails(examId) {
  const { data } = await api.get(`/api/exams/${examId}/details`);

  return data;
}

export async function startSession(examId) {
  const { data } = await api.post('/api/session/start', null, {
    params: { examId },
  });

  if (data?.sessionId && data?.questions) {
    saveSessionQuestions(data.sessionId, data.questions);
  }

  return data;
}

export async function finishSession(sessionId, sessionResponses) {
  const { data } = await api.post('/api/session/finish', {
    sessionId,
    sessionResponses,
  });
  return data;
}

export function abandonSession(sessionId) {
  const token = localStorage.getItem(ACCESS_TOKEN_KEY);
  const baseUrl = api.defaults.baseURL || '';
  fetch(`${baseUrl}/api/session/abandon`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      ...(token ? { Authorization: `Bearer ${token}` } : {}),
    },
    body: JSON.stringify({ sessionId }),
    keepalive: true,
  }).catch(() => {});
}
