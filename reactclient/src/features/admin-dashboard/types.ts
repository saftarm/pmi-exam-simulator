import type { AttemptVolumeDto, PassRateAnalyticsDto } from '../admin-analytics/types';
import type { ExamOverviewStatsDto, ExamSummaryDto } from '../admin-exams/types';
import type { UserListItemDto } from '../admin-users/types';

/** Aggregated dashboard data loaded by useAdminDashboardData. */
export interface AdminDashboardData {
  exams: ExamSummaryDto[];
  examStats: ExamOverviewStatsDto[];
  totalUsers: number;
  recentUsers: UserListItemDto[];
  categoryCount: number;
  usersByRole: Record<string, number>;
  usersByStatus: Record<string, number>;
  attemptVolume: AttemptVolumeDto[];
  passRate: PassRateAnalyticsDto | null;
  loading: boolean;
  loadError: string | null;
}

export interface AdminDashboardDataOptions {
  includeAnalytics?: boolean;
  recentUsersPageSize?: number;
}
