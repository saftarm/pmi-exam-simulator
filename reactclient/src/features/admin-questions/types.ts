import type { ApiId, PaginationParams } from '../../shared/api/primitives';

export type QuestionType = 'SingleChoice' | 'MultipleChoice' | 'TrueFalse';

export interface AnswerOptionDto {
  id: ApiId;
  text: string;
  isCorrect: boolean;
}

export interface QuestionListItemDto {
  id: ApiId;
  title: string | null;
  questionType: QuestionType;
  domainId: ApiId;
  domainTitle: string;
  examTitle: string;
  answerOptionCount: number;
}

export interface QuestionAdminDto {
  id: ApiId;
  title: string | null;
  explanation: string | null;
  questionType: QuestionType;
  domainId: ApiId;
  domainTitle: string;
  examTitle: string;
  answerOptions: AnswerOptionDto[] | null;
}

export interface CreateAnswerOptionDto {
  text: string;
  isCorrect: boolean;
}

export interface UpdateAnswerOptionDto {
  id?: ApiId;
  text: string;
  isCorrect: boolean;
}

export interface CreateQuestionDto {
  title?: string | null;
  explanation?: string | null;
  questionType: QuestionType;
  domainId: ApiId;
  answerOptionsDtos: CreateAnswerOptionDto[];
}

export interface UpdateQuestionRequest {
  title?: string | null;
  explanation?: string | null;
  questionType: QuestionType;
  answerOptionsDtos: UpdateAnswerOptionDto[];
}

export interface BulkDeleteQuestionsRequest {
  questionIds: ApiId[];
}

export interface ImportRowErrorDto {
  row: number;
  reason: string;
}

export interface QuestionImportResultDto {
  success: boolean;
  importedCount: number;
  errors: ImportRowErrorDto[];
}

export interface QuestionListQueryParams extends PaginationParams {
  domainId?: ApiId;
  questionType?: QuestionType;
  search?: string;
}

/** Local admin question form state (Phase 5). */
export interface QuestionFormAnswerOption {
  id?: ApiId;
  text: string;
  isCorrect: boolean;
}

export interface QuestionFormState {
  title: string;
  explanation: string;
  questionType: QuestionType;
  domainId: ApiId | '';
  answerOptions: QuestionFormAnswerOption[];
}
