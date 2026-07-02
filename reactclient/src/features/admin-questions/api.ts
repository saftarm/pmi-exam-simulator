import api from '../../shared/api/client';
import type { ApiId, PagedResponse } from '../../shared/api/primitives';
import type {
  BulkDeleteQuestionsRequest,
  CreateQuestionDto,
  QuestionAdminDto,
  QuestionImportResultDto,
  QuestionListItemDto,
  QuestionListQueryParams,
  UpdateQuestionRequest,
} from './types';

export async function getQuestions(
  params: QuestionListQueryParams = {},
): Promise<PagedResponse<QuestionListItemDto>> {
  const { data } = await api.get<PagedResponse<QuestionListItemDto>>('/api/questions', { params });
  return data;
}

export async function importQuestions(file: File): Promise<QuestionImportResultDto> {
  const formData = new FormData();
  formData.append('file', file);
  const { data } = await api.post<QuestionImportResultDto>('/api/questions/import', formData, {
    headers: { 'Content-Type': 'multipart/form-data' },
  });
  return data;
}

export async function getQuestion(id: ApiId): Promise<QuestionAdminDto> {
  const { data } = await api.get<QuestionAdminDto>(`/api/questions/${id}`);
  return data;
}

export async function createQuestion(payload: CreateQuestionDto): Promise<void> {
  await api.post('/api/questions', payload);
}

export async function updateQuestion(id: ApiId, payload: UpdateQuestionRequest): Promise<void> {
  await api.put(`/api/questions/${id}`, { ...payload, id });
}

export async function deleteQuestion(id: ApiId): Promise<void> {
  await api.delete(`/api/questions/${id}`);
}

export async function bulkDeleteQuestions(questionIds: ApiId[]): Promise<void> {
  const body: BulkDeleteQuestionsRequest = { questionIds };
  await api.delete('/api/questions', { data: body });
}
