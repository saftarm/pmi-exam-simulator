/**
 * Cross-feature API primitives shared by unrelated feature slices.
 * Feature-specific DTOs belong in features/<feature>/types.ts.
 */

/** GUID serialized as a string in JSON. */
export type ApiId = string;

/** ISO 8601 date-time string from ASP.NET Core DateTime serialization. */
export type IsoDateTimeString = string;

/** ISO 8601 date string from ASP.NET Core DateOnly serialization (YYYY-MM-DD). */
export type IsoDateString = string;

export interface PagedResponse<T> {
  items: T[];
  page: number;
  pageSize: number;
  totalCount: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}

export interface PaginationParams {
  pageNumber?: number;
  pageSize?: number;
}

/** FluentValidation-style validation item (422 responses). */
export interface ValidationError {
  propertyName: string;
  errorMessage: string;
}

/** RFC 7807 / ASP.NET Core ProblemDetails (500 and some error paths). */
export interface ProblemDetails {
  type?: string;
  title?: string;
  status?: number;
  detail?: string;
  instance?: string;
  errors?: ValidationError[];
}
