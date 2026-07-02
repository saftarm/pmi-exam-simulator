import api, { ACCESS_TOKEN_KEY } from '../../shared/api/client';
import type { ApiId } from '../../shared/api/primitives';
import type {
  FinishSessionRequest,
  SessionDto,
  SessionResultDto,
  UserExamResponseDto,
} from './types';
import { saveSessionQuestions } from './utils/sessionStorage';

export async function startSession(examId: ApiId): Promise<SessionDto> {
  const { data } = await api.post<SessionDto>('/api/session/start', null, {
    params: { examId },
  });

  if (data?.sessionId && data?.questions) {
    saveSessionQuestions(data.sessionId, data.questions);
  }

  return data;
}

export async function finishSession(
  sessionId: ApiId,
  sessionResponses: UserExamResponseDto[],
): Promise<SessionResultDto> {
  const body: FinishSessionRequest = { sessionId, sessionResponses };
  const { data } = await api.post<SessionResultDto>('/api/session/finish', body);
  return data;
}

export function abandonSession(sessionId: ApiId) {
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
