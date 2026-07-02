import type { ApiId } from '../../shared/api/primitives';

export interface ExamDetailsDto {
  id: ApiId;
  title: string | null;
  context: string | null;
  durationInMinutes: number;
  numberOfQuestions: number;
  attemptCount: number;
  isMostPopular: boolean;
}

/** Exam metadata passed into the session route via router state. */
export interface LearnerExamMeta {
  id: ApiId;
  title: string | null;
  durationInMinutes: number;
  numberOfQuestions: number;
}

export interface LearnerExamDomainDto {
  id: ApiId;
  title: string | null;
  description: string | null;
  weight: number;
  examId: ApiId;
}

/**
 * GET /api/domains/withTitles?examId={examId} returns Dictionary<Guid, string>.
 * Learner exam views own this shape separately from admin category management.
 */
export type LearnerExamDomainTitlesMap = Record<ApiId, string>;
