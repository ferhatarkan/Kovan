import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export class FormValidators {
  static requiredIf(condition: () => boolean): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!condition()) {
        return null;
      }
      return control.value ? null : { required: true };
    };
  }

  static minLength(minLength: number): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value;
      if (!value) return null;
      return value.length >= minLength ? null : { minlength: { requiredLength: minLength, actualLength: value.length } };
    };
  }

  static maxLength(maxLength: number): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value;
      if (!value) return null;
      return value.length <= maxLength ? null : { maxlength: { requiredLength: maxLength, actualLength: value.length } };
    };
  }

  static email(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value;
      if (!value) return null;
      const emailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
      return emailPattern.test(value) ? null : { email: true };
    };
  }

  static min(minValue: number): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value;
      if (value === null || value === undefined || value === '') return null;
      const numValue = typeof value === 'string' ? parseFloat(value) : value;
      return numValue >= minValue ? null : { min: { min: minValue, actual: numValue } };
    };
  }

  static max(maxValue: number): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value;
      if (value === null || value === undefined || value === '') return null;
      const numValue = typeof value === 'string' ? parseFloat(value) : value;
      return numValue <= maxValue ? null : { max: { max: maxValue, actual: numValue } };
    };
  }

  static pattern(pattern: string | RegExp): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value;
      if (!value) return null;
      const regex = typeof pattern === 'string' ? new RegExp(pattern) : pattern;
      return regex.test(value) ? null : { pattern: { requiredPattern: pattern } };
    };
  }

  static turkishTaxNumber(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value;
      if (!value) return null;
      const taxNumberPattern = /^[0-9]{10}$/;
      return taxNumberPattern.test(value) ? null : { taxNumber: true };
    };
  }

  static turkishIdentityNumber(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value;
      if (!value) return null;
      const identityPattern = /^[0-9]{11}$/;
      return identityPattern.test(value) ? null : { identityNumber: true };
    };
  }

  static phone(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value;
      if (!value) return null;
      const phonePattern = /^[0-9]{10,15}$/;
      return phonePattern.test(value) ? null : { phone: true };
    };
  }
}