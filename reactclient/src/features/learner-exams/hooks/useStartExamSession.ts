import { useState, useCallback } from 'react';
import { isAxiosError } from 'axios';
import { useNavigate } from 'react-router-dom';
import { startSession } from '../../exam-session/api';
import { getSessionErrorMessage } from '../../exam-session/utils/sessionAnswers';
import type { ApiId } from '../../../shared/api/primitives';
import type { LearnerExamMeta } from '../types';

export function useStartExamSession() {
  const navigate = useNavigate();
  const [startingId, setStartingId] = useState<ApiId | null>(null);
  const [startError, setStartError] = useState<string | null>(null);

  const startExam = useCallback(
    async (examId: ApiId, exam: LearnerExamMeta) => {
      setStartingId(examId);
      setStartError(null);
      try {
        const session = await startSession(examId);
        navigate(`/exams/${examId}/session/${session.sessionId}`, {
          state: { exam },
        });
        return session;
      } catch (err: unknown) {
        let message = getSessionErrorMessage(err, 'start');
        if (isAxiosError(err) && err.response?.status === 403) {
          message = 'This exam is not available for practice yet.';
        }
        setStartError(message);
        throw err;
      } finally {
        setStartingId(null);
      }
    },
    [navigate],
  );

  const clearStartError = useCallback(() => setStartError(null), []);

  return {
    startExam,
    startingId,
    starting: !!startingId,
    startError,
    clearStartError,
    setStartError,
  };
}
