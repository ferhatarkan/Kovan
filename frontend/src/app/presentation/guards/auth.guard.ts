import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

import { TokenStorageService } from '../../core/infrastructure/services/token-storage.service';

/**
 * Kimliği doğrulanmamış kullanıcıları login sayfasına yönlendirir.
 */
export const authGuard: CanActivateFn = () => {
  const tokenStorage = inject(TokenStorageService);
  const router = inject(Router);

  if (tokenStorage.getToken()) {
    return true;
  }

  return router.createUrlTree(['/auth/login']);
};