import api from '../../shared/api/client';
import type { DomainPerformanceDto } from './types';

export async function getDomainPerformances(): Promise<DomainPerformanceDto[]> {
  const { data } = await api.get<DomainPerformanceDto[]>('/api/progress/domains');
  return data;
}
