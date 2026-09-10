import { inject, Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';

import { AuthRepository } from '../repositories/auth.repository';
import { AuthResponse, LoginRequest, RegisterRequest } from '../models/auth.model';

/**
 * Kullanıcı girişi use case'i.
 */
@Injectable({ providedIn: 'root' })
export class LoginUseCase {
  private readonly authRepository = inject(AuthRepository);

  execute(request: LoginRequest): Observable<AuthResponse> {
    return this.authRepository.login(request);
  }
}

/**
 * Yeni kullanıcı kaydı use case'i.
 */
@Injectable({ providedIn: 'root' })
export class RegisterUseCase {
  private readonly authRepository = inject(AuthRepository);

  execute(request: RegisterRequest): Observable<AuthResponse> {
    return this.authRepository.register(request);
  }
}

/**
 * Oturum kapatma use case'i.
 */
@Injectable({ providedIn: 'root' })
export class LogoutUseCase {
  private readonly authRepository = inject(AuthRepository);

  execute(): Observable<void> {
    return this.authRepository.logout();
  }
}