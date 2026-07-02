import { useState, useEffect, useCallback } from 'react';
import { getDomainPerformances } from '../api';
import type { DomainPerformanceDto } from '../types';

let cachedData: DomainPerformanceDto[] | null = null;
let cachedUserId: string | null = null;
let inflightPromise: Promise<DomainPerformanceDto[]> | null = null;

export function invalidateDomainPerformances() {
  cachedData = null;
  cachedUserId = null;
  inflightPromise = null;
}

function fetchPerformances(userId: string) {
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
    .catch((err: unknown) => {
      inflightPromise = null;
      throw err;
    });
  return inflightPromise;
}

interface PerformanceSnapshot {
  userId: string | null;
  performances: DomainPerformanceDto[];
  error: string | null;
  pending: boolean;
}

const EMPTY_SNAPSHOT: PerformanceSnapshot = {
  userId: null,
  performances: [],
  error: null,
  pending: false,
};

export function useDomainPerformances(userId?: string | null, { silent = false }: { silent?: boolean } = {}) {
  const [snapshot, setSnapshot] = useState<PerformanceSnapshot>(EMPTY_SNAPSHOT);

  const reload = useCallback(() => {
    if (!userId) return Promise.resolve([] as DomainPerformanceDto[]);
    invalidateDomainPerformances();
    setSnapshot({ userId, performances: [], error: null, pending: true });
    return fetchPerformances(userId)
      .then((data) => {
        setSnapshot({ userId, performances: data, error: null, pending: false });
        return data;
      })
      .catch(() => {
        const message = 'Failed to load progress data.';
        setSnapshot({
          userId,
          performances: [],
          error: silent ? null : message,
          pending: false,
        });
        return [] as DomainPerformanceDto[];
      });
  }, [userId, silent]);

  useEffect(() => {
    if (!userId) {
      return;
    }

    let cancelled = false;

    fetchPerformances(userId)
      .then((data) => {
        if (!cancelled) {
          setSnapshot({ userId, performances: data, error: null, pending: false });
        }
      })
      .catch(() => {
        if (!cancelled) {
          setSnapshot({
            userId,
            performances: [],
            error: silent ? null : 'Failed to load progress data.',
            pending: false,
          });
        }
      });

    return () => {
      cancelled = true;
    };
  }, [userId, silent]);

  if (!userId) {
    return {
      performances: [] as DomainPerformanceDto[],
      loading: false,
      error: null as string | null,
      reload: () => Promise.resolve([] as DomainPerformanceDto[]),
    };
  }

  const isCurrentUser = snapshot.userId === userId;
  const loading = !isCurrentUser || snapshot.pending;
  const performances = isCurrentUser ? snapshot.performances : [];
  const error = isCurrentUser ? snapshot.error : null;

  return { performances, loading, error, reload };
}

export function usePerformanceSummary(performances: DomainPerformanceDto[]) {
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
