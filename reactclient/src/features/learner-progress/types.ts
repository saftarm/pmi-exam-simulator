import type { ApiId, IsoDateTimeString } from '../../shared/api/primitives';

export interface DomainPerformanceDto {
  domainId: ApiId;
  domainTitle: string;
  examId: ApiId;
  examTitle: string;
  totalAnswered: number;
  totalCorrect: number;
  percentageScore: number;
  lastUpdated: IsoDateTimeString;
}
