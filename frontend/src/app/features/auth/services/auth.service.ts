import { Injectable, computed, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { tap } from 'rxjs';

import { AuthResponse } from '../../../core/domain/models/auth.model';
import { LoginUseCase, LogoutUseCase } from '../../../core/domain/usecases/auth.usecases';
import { TokenStorageService } from '../../../core/infrastructure/services/token-storage.service';

/**
 * Oturum durumunu yönetir (feature-level application service).
 */
@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly loginUseCase = inject(LoginUseCase);
  private readonly logoutUseCase = inject(LogoutUseCase);
  private readonly tokenStorage = inject(TokenStorageService);
  private readonly router = inject(Router);

  private readonly _isAuthenticated = signal<boolean>(!!this.tokenStorage.getToken());

  /** Reaktif oturum durumu. */
  readonly isAuthenticated = computed(() => this._isAuthenticated());

  login(email: string, password: string) {
    return this.loginUseCase.execute({ email, password }).pipe(
      tap((response: AuthResponse) => {
        if (response.token && response.refreshToken) {
          this.tokenStorage.setSession(response.token, response.refreshToken);
          this._isAuthenticated.set(true);
        }
      }),
    );
  }

  logout(): void {
    this.logoutUseCase.execute().subscribe({
      complete: () => this.finishLogout(),
      error: () => this.finishLogout(),
    });
  }

  private finishLogout(): void {
    this.tokenStorage.clear();
    this._isAuthenticated.set(false);
    this.router.navigate(['/auth/login']);
  }
}