import type { ApiId } from '../../shared/api/primitives';

export type ExamStatus = 'Draft' | 'Published' | 'Archived';

export interface ExamSummaryDto {
  id: ApiId;
  title: string;
  categoryTitle: string;
  categoryId: ApiId;
  numberOfQuestions: number;
  durationInMinutes: number;
  status: ExamStatus;
}

export interface CreateExamDomainDto {
  title: string;
  description: string;
  weight: number;
}

export interface CreateExamDto {
  categoryId: ApiId;
  title: string;
  context: string;
  durationInMinutes: number;
  numberOfQuestions: number;
  createDomainDtos: CreateExamDomainDto[];
}

export interface UpdateExamRequest {
  numberOfQuestions: number;
  durationInMinutes: number;
}

export interface BulkDeleteExamsRequest {
  examIds: ApiId[];
}

export interface ExamOverviewStatsDto {
  examId: ApiId;
  examTitle: string;
  attemptCount: number;
  uniqueUsersCount: number;
  averageScore: number;
}
