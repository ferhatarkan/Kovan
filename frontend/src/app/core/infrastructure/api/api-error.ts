import { HttpErrorResponse } from '@angular/common/http';

/**
 * Backend'deki ProblemDetails (application/problem+json) yanıtını
 * tip güvenli bir şekilde temsil eder.
 */
export interface ProblemDetails {
  type?: string;
  title?: string;
  status?: number;
  detail?: string;
  instance?: string;
  errors?: Record<string, string[]>;
  [key: string]: unknown;
}

/**
 * Backend'den gelen hata yanıtlarını anlamlı bir istemci hatasına dönüştürür.
 */
export class ApiError extends Error {
  readonly status: number;
  readonly title: string;
  readonly detail?: string;
  readonly errors?: Record<string, string[]>;

  constructor(status: number, problem: ProblemDetails) {
    super(problem.detail ?? problem.title ?? 'Beklenmedik bir hata oluştu.');
    this.name = 'ApiError';
    this.status = status;
    this.title = problem.title ?? 'Hata';
    this.detail = problem.detail;
    this.errors = problem.errors;
  }

  static fromHttpError(error: HttpErrorResponse): ApiError {
    const problem = (typeof error.error === 'object' && error.error !== null
      ? error.error
      : {}) as ProblemDetails;
    return new ApiError(error.status, problem);
  }
}