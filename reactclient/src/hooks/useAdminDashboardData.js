import { useEffect, useState } from 'react';
import { getAllExams, getExamOverviewStats } from '../services/adminExamService';
import { getCategories } from '../services/adminCategoryService';
import { getAttemptVolume, getPassRateAnalytics } from '../services/adminAnalyticsService';
import { getUserCount, getUserStats, getUsers } from '../services/adminUserService';

export function useAdminDashboardData({ includeAnalytics = false, recentUsersPageSize = 6 } = {}) {
  const [exams, setExams] = useState([]);
  const [examStats, setExamStats] = useState([]);
  const [totalUsers, setTotalUsers] = useState(0);
  const [recentUsers, setRecentUsers] = useState([]);
  const [categoryCount, setCategoryCount] = useState(0);
  const [usersByRole, setUsersByRole] = useState({});
  const [usersByStatus, setUsersByStatus] = useState({});
  const [attemptVolume, setAttemptVolume] = useState([]);
  const [passRate, setPassRate] = useState(null);
  const [loading, setLoading] = useState(true);
  const [loadError, setLoadError] = useState(null);

  useEffect(() => {
    let failed = false;
    const baseFetches = [
      getAllExams().catch(() => {
        failed = true;
        return [];
      }),
      getExamOverviewStats().catch(() => {
        failed = true;
        return [];
      }),
      getUserCount().catch(() => {
        failed = true;
        return 0;
      }),
      getUsers({ pageNumber: 1, pageSize: recentUsersPageSize }).catch(() => {
        failed = true;
        return { items: [] };
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
            return [];
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
