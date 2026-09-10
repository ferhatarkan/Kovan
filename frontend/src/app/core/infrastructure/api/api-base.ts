import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { environment } from '../../../../environments/environment';
import { ApiError } from './api-error';

/**
 * Tüm API repository implementasyonları için ortak taban sınıf.
 * API URL'ini ve hata dönüşümünü merkezi olarak yönetir.
 */
export abstract class ApiBase {
  protected readonly http: HttpClient = inject(HttpClient);
  protected readonly baseUrl: string = environment.apiUrl;

  /**
   * Backend'den gelen HttpErrorResponse'ları ApiError'a dönüştürür.
   */
  protected toApiError<T>(source: Observable<T>): Observable<T> {
    return source.pipe(
      catchError((error: unknown) => {
        if (error instanceof HttpErrorResponse) {
          return throwError(() => ApiError.fromHttpError(error));
        }
        return throwError(() => error);
      }),
    );
  }
}