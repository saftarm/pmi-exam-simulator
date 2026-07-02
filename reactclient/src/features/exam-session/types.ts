import type { ApiId } from '../../shared/api/primitives';
import type { AnswerOptionDto, QuestionType } from '../admin-questions/types';

export interface SessionDto {
  sessionId: ApiId;
  questions: QuestionSnapshotDto[];
}

/**
 * Session question snapshot returned by POST /api/session/start.
 *
 * TODO(API.md mismatch): docs/API.md lists `id`, `title`, and `answerOptionsDtos`.
 * The backend QuestionSnapshotDto serializes as `questionId`, `questionTitle`,
 * `domainId`, and `answerOptions` (see DTO/Question/QuestionSnapshotDto.cs).
 * The React client already consumes the backend wire names.
 */
export interface QuestionSnapshotDto {
  questionId: ApiId;
  questionTitle: string | null;
  questionType: QuestionType;
  domainId: ApiId;
  answerOptions: AnswerOptionDto[] | null;
}

export interface UserExamResponseDto {
  questionId: ApiId;
  selectedOptionIds: ApiId[];
}

export interface FinishSessionRequest {
  sessionId: ApiId;
  sessionResponses: UserExamResponseDto[];
}

export interface AbandonSessionRequest {
  sessionId: ApiId;
}

export interface SessionResultDto {
  scorePoints: number;
  percentageScore: number;
}

export type { QuestionType } from '../admin-questions/types';
