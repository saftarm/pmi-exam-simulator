import { useEffect, useState } from 'react';
import { getAllExams, getExamOverviewStats } from '../../admin-exams/api';
import type { ExamOverviewStatsDto, ExamSummaryDto } from '../../admin-exams/types';
import { getCategories } from '../../admin-categories/api';
import { getAttemptVolume, getPassRateAnalytics } from '../../admin-analytics/api';
import type { AttemptVolumeDto, PassRateAnalyticsDto } from '../../admin-analytics/types';
import { getUserCount, getUserStats, getUsers } from '../../admin-users/api';
import type { UserListItemDto } from '../../admin-users/types';
import type { AdminDashboardDataOptions } from '../types';

export function useAdminDashboardData({
  includeAnalytics = false,
  recentUsersPageSize = 6,
}: AdminDashboardDataOptions = {}) {
  const [exams, setExams] = useState<ExamSummaryDto[]>([]);
  const [examStats, setExamStats] = useState<ExamOverviewStatsDto[]>([]);
  const [totalUsers, setTotalUsers] = useState(0);
  const [recentUsers, setRecentUsers] = useState<UserListItemDto[]>([]);
  const [categoryCount, setCategoryCount] = useState(0);
  const [usersByRole, setUsersByRole] = useState<Record<string, number>>({});
  const [usersByStatus, setUsersByStatus] = useState<Record<string, number>>({});
  const [attemptVolume, setAttemptVolume] = useState<AttemptVolumeDto[]>([]);
  const [passRate, setPassRate] = useState<PassRateAnalyticsDto | null>(null);
  const [loading, setLoading] = useState(true);
  const [loadError, setLoadError] = useState<string | null>(null);

  useEffect(() => {
    let failed = false;
    const baseFetches = [
      getAllExams().catch(() => {
        failed = true;
        return [] as ExamSummaryDto[];
      }),
      getExamOverviewStats().catch(() => {
        failed = true;
        return [] as ExamOverviewStatsDto[];
      }),
      getUserCount().catch(() => {
        failed = true;
        return 0;
      }),
      getUsers({ pageNumber: 1, pageSize: recentUsersPageSize }).catch(() => {
        failed = true;
        return { items: [] as UserListItemDto[] };
      }),
    ];

    const analyticsFetches = includeAnalytics
      ? [
          getCategories().catch(() => {
            failed = true;
            return [];
          }),
          getUserStats().catch(() => {
            failed = true;
            return { byRole: {}, byStatus: {} };
          }),
          getAttemptVolume(30).catch(() => {
            failed = true;
            return [] as AttemptVolumeDto[];
          }),
          getPassRateAnalytics().catch(() => {
            failed = true;
            return null;
          }),
        ]
      : [];

    Promise.all([...baseFetches, ...analyticsFetches])
      .then((results) => {
        const [examData, stats, count, usersData, ...analyticsResults] = results;
        setExams(examData);
        setExamStats(stats);
        setTotalUsers(count);
        setRecentUsers(usersData.items || []);

        if (includeAnalytics && analyticsResults.length === 4) {
          const [categories, userStats, volume, pass] = analyticsResults;
          setCategoryCount(categories.length);
          setUsersByRole(userStats.byRole ?? {});
          setUsersByStatus(userStats.byStatus ?? {});
          setAttemptVolume(volume);
          setPassRate(pass);
        }

        if (failed) {
          setLoadError(
            includeAnalytics
              ? 'Could not load all report data. Showing partial results.'
              : 'Could not load all dashboard data. Showing partial results.',
          );
        }
      })
      .finally(() => setLoading(false));
  }, [includeAnalytics, recentUsersPageSize]);

  return {
    exams,
    examStats,
    totalUsers,
    recentUsers,
    categoryCount,
    usersByRole,
    usersByStatus,
    attemptVolume,
    passRate,
    loading,
    loadError,
    setLoadError,
  };
}
