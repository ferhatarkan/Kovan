import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { ApiError } from '../../infrastructure/api/api-error';

export interface ValidationResult {
  isValid: boolean;
  errors: Record<string, string>;
}

@Injectable({ providedIn: 'root' })
export abstract class BaseValidator<T> {
  abstract validate(request: T): ValidationResult;

  throwIfInvalid(validationResult: ValidationResult): Observable<void> {
    if (!validationResult.isValid) {
      return throwError(() =>
        new ApiError(400, {
          title: 'VALIDATION_ERROR',
          detail: Object.values(validationResult.errors).join(', '),
        })
      );
    }
    return new Observable<void>((observer) => {
      observer.next();
      observer.complete();
    });
  }

  validateRequest(request: T): Observable<void> {
    const validationResult = this.validate(request);
    return this.throwIfInvalid(validationResult);
  }
}