import { Observable } from 'rxjs';

import { AuthResponse, LoginRequest, RegisterRequest } from '../models/auth.model';

/**
 * Kimlik doğrulama işlemleri için domain port'u.
 * Infrastructure katmanı bu arayüzü implemente eder.
 */
export abstract class AuthRepository {
  abstract login(request: LoginRequest): Observable<AuthResponse>;
  abstract register(request: RegisterRequest): Observable<AuthResponse>;
  abstract refreshToken(refreshToken: string): Observable<AuthResponse>;
  abstract logout(): Observable<void>;
}