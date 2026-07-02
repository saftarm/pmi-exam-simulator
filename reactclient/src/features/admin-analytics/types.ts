import type { IsoDateString } from '../../shared/api/primitives';

export type AttemptStatus = 'InProgress' | 'Completed' | 'Abandoned';

export interface AttemptVolumeDto {
  date: IsoDateString;
  count: number;
}

export interface PassRateAnalyticsDto {
  averageScore: number;
  totalCompletedAttempts: number;
  passCount: number;
  passRate: number;
  passThreshold: number;
}
