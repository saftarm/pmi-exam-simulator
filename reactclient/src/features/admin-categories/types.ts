import type { ApiId } from '../../shared/api/primitives';
import type { ExamSummaryDto } from '../admin-exams/types';

export interface CategoryDto {
  id: ApiId;
  title: string | null;
  description: string | null;
  numberOfExams: number;
  examSummaryDtos: ExamSummaryDto[] | null;
}

export interface CreateCategoryDto {
  title?: string | null;
  description?: string | null;
}

export interface UpdateCategoryRequest {
  title: string;
  description: string;
}

export interface DomainDto {
  id: ApiId;
  title: string | null;
  description: string | null;
  weight: number;
  examId: ApiId;
}

export interface CreateDomainDto {
  examId?: ApiId;
  title: string;
  description: string;
  weight: number;
}

export interface UpdateDomainDto {
  title: string;
  description: string;
  weight: number;
}

/**
 * GET /api/domains/withTitles?examId={examId} returns Dictionary<Guid, string>.
 * JSON object keys are GUID strings mapped to domain titles.
 */
export type DomainTitlesMap = Record<ApiId, string>;
