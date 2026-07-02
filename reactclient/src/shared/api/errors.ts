import type { AxiosError } from 'axios';
import type { ProblemDetails, ValidationError } from './primitives';

/**
 * Raw error body shapes returned by the API.
 * Many endpoints return empty bodies for 400/401/403/404/409 success paths.
 */
export type ApiErrorBody =
  | string
  | ValidationError[]
  | ProblemDetails
  | null
  | undefined;

/** Normalized error message after client-side parsing. */
export type NormalizedApiError = string;

function extractValidationMessages(items: Array<{ errorMessage?: string; message?: string }>) {
  return items.map((e) => e.errorMessage || e.message || String(e)).join(' ');
}

/**
 * Normalize plain-string, validation-array, ProblemDetails, and empty API errors.
 * Mirrors the current formatApiErrors behavior in authService.js.
 */
export function normalizeApiError(error: AxiosError<ApiErrorBody> | Error): NormalizedApiError {
  if (!('response' in error) || !error.response) {
    return error.message || 'Something went wrong';
  }

  const data = error.response.data;
  if (!data) return error.message || 'Something went wrong';
  if (typeof data === 'string') return data;
  if (Array.isArray(data)) return extractValidationMessages(data);
  if (data.errors && Array.isArray(data.errors)) return extractValidationMessages(data.errors);
  if (data.detail) return data.detail;
  if (data.title) return data.title;
  return 'Request failed';
}

/** @deprecated Prefer normalizeApiError — kept for existing call sites during migration. */
export const formatApiErrors = normalizeApiError;
