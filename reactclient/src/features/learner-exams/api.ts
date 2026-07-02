import api from '../../shared/api/client';
import type { ApiId, PagedResponse } from '../../shared/api/primitives';
import type {
  ExamDetailsDto,
  LearnerExamDomainDto,
  LearnerExamDomainTitlesMap,
} from './types';

export async function getPublishedExams({
  pageNumber = 1,
  pageSize = 50,
}: { pageNumber?: number; pageSize?: number } = {}) {
  const { data } = await api.get<PagedResponse<ExamDetailsDto> | ExamDetailsDto[]>(
    '/api/exams/details',
    { params: { pageNumber, pageSize } },
  );
  return data;
}

export async function getExamDetails(examId: ApiId): Promise<ExamDetailsDto> {
  const { data } = await api.get<ExamDetailsDto>(`/api/exams/${examId}/details`);
  return data;
}

function normalizeDomainEntries(
  data: LearnerExamDomainTitlesMap | LearnerExamDomainDto[] | null | undefined,
) {
  if (!data) return [] as [ApiId, string][];
  if (Array.isArray(data)) {
    return data.map((d) => [d.id, d.title ?? ''] as [ApiId, string]);
  }
  return Object.entries(data) as [ApiId, string][];
}

export async function getDomainsByExam(examId: ApiId): Promise<LearnerExamDomainDto[]> {
  const { data } = await api.get<LearnerExamDomainTitlesMap>('/api/domains/withTitles', {
    params: { examId },
  });
  const entries = normalizeDomainEntries(data);
  if (entries.length === 0) return [];

  const domains = await Promise.all(
    entries.map(async ([id, title]) => {
      try {
        return await getDomain(id);
      } catch {
        return { id, title, description: '', weight: 1, examId } satisfies LearnerExamDomainDto;
      }
    }),
  );
  return domains;
}

async function getDomain(id: ApiId): Promise<LearnerExamDomainDto> {
  const { data } = await api.get<LearnerExamDomainDto>(`/api/domains/${id}`);
  return data;
}

