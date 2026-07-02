import api from '../../shared/api/client';
import type { ApiId } from '../../shared/api/primitives';
import type {
  BulkDeleteExamsRequest,
  CreateExamDto,
  ExamOverviewStatsDto,
  ExamSummaryDto,
  UpdateExamRequest,
} from './types';
import type { ExamDetailsDto } from '../learner-exams/types';

export async function getAllExams(): Promise<ExamSummaryDto[]> {
  const { data } = await api.get<ExamSummaryDto[]>('/api/exams');
  return data;
}

export async function getAdminExamDetails(examId: ApiId): Promise<ExamDetailsDto> {
  const { data } = await api.get<ExamDetailsDto>(`/api/exams/${examId}/details`);
  return data;
}

export async function createExam(payload: CreateExamDto): Promise<void> {
  await api.post('/api/exams', payload);
}

export async function updateExam(examId: ApiId, payload: UpdateExamRequest): Promise<void> {
  await api.patch(`/api/exams/${examId}/update`, payload);
}

export async function publishExam(examId: ApiId): Promise<void> {
  await api.post(`/api/exams/${examId}/publish`);
}

export async function archiveExam(examId: ApiId): Promise<void> {
  await api.post(`/api/exams/${examId}/archive`);
}

export async function deleteExam(examId: ApiId): Promise<void> {
  await api.delete(`/api/exams/${examId}`);
}

export async function deleteExamsBulk(examIds: ApiId[]): Promise<void> {
  const body: BulkDeleteExamsRequest = { examIds };
  await api.delete('/api/exams', { data: body });
}

export async function getExamOverviewStats(): Promise<ExamOverviewStatsDto[]> {
  const { data } = await api.get<ExamOverviewStatsDto[]>('/api/admin/exams/stats');
  return data;
}
