import { useState, useEffect, useCallback } from 'react';
import { getDomainPerformances } from '../services/progressService';

let cachedData = null;
let cachedUserId = null;
let inflightPromise = null;

export function invalidateDomainPerformances() {
  cachedData = null;
  cachedUserId = null;
  inflightPromise = null;
}

function fetchPerformances(userId) {
  if (cachedData !== null && cachedUserId === userId) {
    return Promise.resolve(cachedData);
  }
  if (inflightPromise && cachedUserId === userId) {
    return inflightPromise;
  }
  cachedUserId = userId;
  inflightPromise = getDomainPerformances()
    .then((data) => {
      cachedData = data || [];
      return cachedData;
    })
    .catch((err) => {
      inflightPromise = null;
      throw err;
    });
  return inflightPromise;
}

export function useDomainPerformances(userId, { silent = false } = {}) {
  const [performances, setPerformances] = useState([]);
  const [loading, setLoading] = useState(!!userId);
  const [error, setError] = useState(null);

  const reload = useCallback(() => {
    if (!userId) return Promise.resolve([]);
    invalidateDomainPerformances();
    setLoading(true);
    setError(null);
    return fetchPerformances(userId)
      .then((data) => {
        setPerformances(data);
        return data;
      })
      .catch(() => {
        const message = 'Failed to load progress data.';
        if (!silent) setError(message);
        setPerformances([]);
        return [];
      })
      .finally(() => setLoading(false));
  }, [userId, silent]);

  useEffect(() => {
    if (!userId) {
      setPerformances([]);
      setLoading(false);
      setError(null);
      return;
    }

    let cancelled = false;
    setLoading(true);
    setError(null);

    fetchPerformances(userId)
      .then((data) => {
        if (!cancelled) setPerformances(data);
      })
      .catch(() => {
        if (!cancelled) {
          if (!silent) setError('Failed to load progress data.');
          setPerformances([]);
        }
      })
      .finally(() => {
        if (!cancelled) setLoading(false);
      });

    return () => {
      cancelled = true;
    };
  }, [userId, silent]);

  return { performances, loading, error, reload };
}

export function usePerformanceSummary(performances) {
  if (!performances.length) return null;
  const examIds = new Set(performances.map((p) => p.examId));
  const avgScore =
    performances.reduce((sum, p) => sum + (p.percentageScore ?? 0), 0) / performances.length;
  return {
    examsAttempted: examIds.size,
    domainsTracked: performances.length,
    averageScore: Math.round(avgScore),
  };
}
