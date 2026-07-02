import api from '../../shared/api/client';
import type { ApiId } from '../../shared/api/primitives';
import type {
  CategoryDto,
  CreateCategoryDto,
  CreateDomainDto,
  DomainDto,
  DomainTitlesMap,
  UpdateCategoryRequest,
  UpdateDomainDto,
} from './types';

export async function getCategories(): Promise<CategoryDto[]> {
  const { data } = await api.get<CategoryDto[]>('/api/categories');
  return data;
}

export async function getCategory(id: ApiId): Promise<CategoryDto> {
  const { data } = await api.get<CategoryDto>(`/api/categories/${id}`);
  return data;
}

export async function createCategory(payload: CreateCategoryDto): Promise<void> {
  await api.post('/api/categories', payload);
}

export async function updateCategory(payload: UpdateCategoryRequest & { categoryId: ApiId }): Promise<void> {
  await api.put(`/api/categories/${payload.categoryId}`, payload);
}

export async function deleteCategory(id: ApiId): Promise<void> {
  await api.delete(`/api/categories/${id}`);
}

export async function getAllDomains(): Promise<DomainDto[]> {
  const { data } = await api.get<DomainDto[]>('/api/domains');
  return Array.isArray(data) ? data : [];
}

function normalizeDomainEntries(data: DomainTitlesMap | DomainDto[] | null | undefined) {
  if (!data) return [] as [ApiId, string][];
  if (Array.isArray(data)) {
    return data.map((d: DomainDto) => [d.id, d.title ?? ''] as [ApiId, string]);
  }
  return Object.entries(data) as [ApiId, string][];
}

export async function getDomainsByExam(examId: ApiId): Promise<DomainDto[]> {
  const { data } = await api.get<DomainTitlesMap>('/api/domains/withTitles', { params: { examId } });
  const entries = normalizeDomainEntries(data);
  if (entries.length === 0) return [];

  return Promise.all(
    entries.map(async ([id, title]) => {
      try {
        return await getDomain(id);
      } catch {
        return { id, title, description: '', weight: 1, examId } satisfies DomainDto;
      }
    }),
  );
}

export async function getDomain(id: ApiId): Promise<DomainDto> {
  const { data } = await api.get<DomainDto>(`/api/domains/${id}`);
  return data;
}

export async function createDomain(examId: ApiId, payload: Omit<CreateDomainDto, 'examId'>): Promise<void> {
  await api.post('/api/domains', { examId, ...payload });
}

export async function updateDomain(id: ApiId, payload: UpdateDomainDto): Promise<void> {
  await api.put(`/api/domains/${id}`, payload);
}

export async function deleteDomain(id: ApiId): Promise<void> {
  await api.delete(`/api/domains/${id}`);
}
