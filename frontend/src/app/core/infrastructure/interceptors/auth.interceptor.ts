import { HttpClient, HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, switchMap, throwError } from 'rxjs';

import { environment } from '../../../../environments/environment';
import { TokenStorageService } from '../services/token-storage.service';

/**
 * Her isteğe Bearer token ekler; 401 durumunda refresh token ile
 * sessiz yenileme dener, başarısızsa oturumu temizler.
 */
export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const tokenStorage = inject(TokenStorageService);
  const http = inject(HttpClient);

  const token = tokenStorage.getToken();
  const authReq = token
    ? req.clone({ setHeaders: { Authorization: `Bearer ${token}` } })
    : req;

  return next(authReq).pipe(
    catchError((error: unknown) => {
      const isAuthEndpoint =
        req.url.includes('/auth/login') || req.url.includes('/auth/refresh-token');
      const shouldRefresh =
        error instanceof HttpErrorResponse &&
        error.status === 401 &&
        !isAuthEndpoint &&
        !!tokenStorage.getRefreshToken();

      if (!shouldRefresh) {
        return throwError(() => error);
      }

      return http
        .post<{ token: string | null; refreshToken: string | null }>(
          `${environment.apiUrl}/auth/refresh-token`,
          { refreshToken: tokenStorage.getRefreshToken() },
        )
        .pipe(
          switchMap((response) => {
            if (!response.token || !response.refreshToken) {
              return throwError(() => error);
            }
            tokenStorage.setSession(response.token, response.refreshToken);
            const retryReq = req.clone({
              setHeaders: { Authorization: `Bearer ${response.token}` },
            });
            return next(retryReq);
          }),
          catchError(() => {
            tokenStorage.clear();
            return throwError(() => error);
          }),
        );
    }),
  );
};