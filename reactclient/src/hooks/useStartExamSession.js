import { useState, useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import { startSession } from '../services/examService';
import { getSessionErrorMessage } from '../utils/sessionAnswers';

export function useStartExamSession() {
  const navigate = useNavigate();
  const [startingId, setStartingId] = useState(null);
  const [startError, setStartError] = useState(null);

  const startExam = useCallback(
    async (examId, exam) => {
      setStartingId(examId);
      setStartError(null);
      try {
        const session = await startSession(examId);
        navigate(`/exams/${examId}/session/${session.sessionId}`, {
          state: { exam },
        });
        return session;
      } catch (err) {
        let message = getSessionErrorMessage(err, 'start');
        if (err.response?.status === 403) {
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
