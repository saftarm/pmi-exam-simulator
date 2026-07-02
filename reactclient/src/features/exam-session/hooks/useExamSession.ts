import { useState, useEffect, useCallback, useRef } from 'react';
import { finishSession, abandonSession } from '../api';
import {
  getSessionErrorMessage,
  countAnswered,
  buildSessionResponses,
  type SessionAnswersMap,
} from '../utils/sessionAnswers';
import { loadSessionQuestions, clearSessionQuestions } from '../utils/sessionStorage';
import { normalizeQuestionType } from '../../admin-questions/utils/questionTypes';
import {
  invalidateDomainPerformances,
  useDomainPerformances,
} from '../../learner-progress/hooks/useDomainPerformances';
import type { DomainPerformanceDto } from '../../learner-progress/types';
import type { ApiId } from '../../../shared/api/primitives';
import type { LearnerExamMeta } from '../../learner-exams/types';
import type { QuestionSnapshotDto, SessionResultDto } from '../types';

interface UseExamSessionParams {
  sessionId: ApiId;
  examId: ApiId;
  examMeta?: LearnerExamMeta | null;
  userId?: ApiId | null;
}

export function useExamSession({ sessionId, examId, examMeta, userId }: UseExamSessionParams) {
  const [questions, setQuestions] = useState<QuestionSnapshotDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [currentIndex, setCurrentIndex] = useState(0);
  const [answers, setAnswers] = useState<SessionAnswersMap>({});
  const [flagged, setFlagged] = useState<Set<number>>(() => new Set());
  const [submitting, setSubmitting] = useState(false);
  const [finished, setFinished] = useState(false);
  const [result, setResult] = useState<SessionResultDto | null>(null);
  const [submitError, setSubmitError] = useState<string | null>(null);
  const [domainResults, setDomainResults] = useState<DomainPerformanceDto[]>([]);

  const finishedRef = useRef(false);
  const answersRef = useRef<SessionAnswersMap>({});
  const questionCountRef = useRef(0);

  const durationMinutes = examMeta?.durationInMinutes ?? 230;
  const { reload: reloadPerformances } = useDomainPerformances(userId, { silent: true });

  useEffect(() => {
    finishedRef.current = finished;
  }, [finished]);

  useEffect(() => {
    answersRef.current = answers;
  }, [answers]);

  useEffect(() => {
    questionCountRef.current = questions.length;
  }, [questions.length]);

  useEffect(() => {
    const handleBeforeUnload = (event: BeforeUnloadEvent) => {
      if (finishedRef.current) return;
      const answered = countAnswered(answersRef.current);
      const totalQuestions = questionCountRef.current;
      if (totalQuestions > 0 && answered < totalQuestions) {
        event.preventDefault();
        event.returnValue = '';
      }
    };

    window.addEventListener('beforeunload', handleBeforeUnload);
    return () => {
      window.removeEventListener('beforeunload', handleBeforeUnload);
      if (finishedRef.current) return;
      const answered = countAnswered(answersRef.current);
      const totalQuestions = questionCountRef.current;
      if (totalQuestions > 0 && answered < totalQuestions && sessionId) {
        abandonSession(sessionId);
        clearSessionQuestions(sessionId);
      }
    };
  }, [sessionId]);

  useEffect(() => {
    const stored = loadSessionQuestions(sessionId);
    if (stored && stored.length > 0) {
      setQuestions(stored);
      setLoading(false);
      return;
    }
    setError('Session questions not found. Please start the exam again from the exam page.');
    setLoading(false);
  }, [sessionId]);

  const handleSelectOption = useCallback(
    (optionId: ApiId) => {
      const currentQuestion = questions[currentIndex];
      const qType = normalizeQuestionType(currentQuestion?.questionType);
      if (qType === 'MultipleChoice') {
        setAnswers((prev) => {
          const current = prev[currentIndex];
          const selected = Array.isArray(current) ? [...current] : current ? [current] : [];
          const idx = selected.indexOf(optionId);
          if (idx >= 0) selected.splice(idx, 1);
          else selected.push(optionId);
          if (selected.length === 0) {
            const next = { ...prev };
            delete next[currentIndex];
            return next;
          }
          return { ...prev, [currentIndex]: selected };
        });
      } else {
        setAnswers((prev) => ({ ...prev, [currentIndex]: optionId }));
      }
    },
    [currentIndex, questions],
  );

  const toggleFlag = useCallback(() => {
    setFlagged((prev) => {
      const next = new Set(prev);
      if (next.has(currentIndex)) next.delete(currentIndex);
      else next.add(currentIndex);
      return next;
    });
  }, [currentIndex]);

  const handleSubmit = useCallback(async () => {
    if (submitting) return;
    const total = questions.length;
    const answeredCount = countAnswered(answers);
    const unanswered = total - answeredCount;
    if (unanswered > 0) {
      const ok = window.confirm(`You have ${unanswered} unanswered question(s). Submit anyway?`);
      if (!ok) return;
    }

    setSubmitting(true);
    setSubmitError(null);
    try {
      const sessionResponses = buildSessionResponses(answers, questions);
      const res = await finishSession(sessionId, sessionResponses);
      clearSessionQuestions(sessionId);
      setResult(res);
      if (userId) {
        try {
          invalidateDomainPerformances();
          const progress = await reloadPerformances();
          const targetExamId = examMeta?.id || examId;
          setDomainResults(
            (progress || []).filter((row) => String(row.examId) === String(targetExamId)),
          );
        } catch {
          setDomainResults([]);
        }
      }
      setFinished(true);
    } catch (err: unknown) {
      setSubmitError(getSessionErrorMessage(err, 'finish'));
    } finally {
      setSubmitting(false);
    }
  }, [
    answers,
    questions,
    sessionId,
    submitting,
    userId,
    examMeta?.id,
    examId,
    reloadPerformances,
  ]);

  const handleExpire = useCallback(() => {
    handleSubmit();
  }, [handleSubmit]);

  const questionsForView = questions.map((q) => ({
    ...q,
    questionType: normalizeQuestionType(q.questionType),
  }));

  return {
    questions: questionsForView,
    loading,
    error,
    currentIndex,
    setCurrentIndex,
    answers,
    flagged,
    submitting,
    finished,
    result,
    submitError,
    domainResults,
    durationMinutes,
    examTitle: examMeta?.title,
    handleSelectOption,
    toggleFlag,
    handleSubmit,
    handleExpire,
  };
}
