import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { AuthRepository } from '../../domain/repositories/auth.repository';
import { AuthResponse, LoginRequest, RegisterRequest } from '../../domain/models/auth.model';
import { TokenStorageService } from '../services/token-storage.service';
import { ApiBase } from '../api/api-base';

/**
 * AuthRepository'nin REST API implementasyonu.
 * Backend endpoint'leri: POST /api/v1/auth/login, register, refresh-token, logout
 */
@Injectable({ providedIn: 'root' })
export class AuthApiRepository extends ApiBase implements AuthRepository {
  private readonly tokenStorage = inject(TokenStorageService);
  private readonly versionedBase = `${this.baseUrl}/v1`;

  login(request: LoginRequest): Observable<AuthResponse> {
    return this.toApiError(this.http.post<AuthResponse>(`${this.versionedBase}/auth/login`, request));
  }

  register(request: RegisterRequest): Observable<AuthResponse> {
    return this.toApiError(this.http.post<AuthResponse>(`${this.versionedBase}/auth/register`, request));
  }

  refreshToken(refreshToken: string): Observable<AuthResponse> {
    return this.toApiError(
      this.http.post<AuthResponse>(`${this.versionedBase}/auth/refresh-token`, { refreshToken }),
    );
  }

  logout(): Observable<void> {
    this.tokenStorage.clear();
    return this.toApiError(this.http.post<void>(`${this.versionedBase}/auth/logout`, {}));
  }
}