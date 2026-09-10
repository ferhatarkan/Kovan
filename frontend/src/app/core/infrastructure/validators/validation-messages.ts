import { AbstractControl, ValidationErrors } from '@angular/forms';

export class ValidationMessages {
  static getErrorMessage(control: AbstractControl, customMessages?: Record<string, string>): string {
    if (!control.errors || !control.touched) {
      return '';
    }

    const errors = control.errors;
    const errorKeys = Object.keys(errors);

    for (const key of errorKeys) {
      if (customMessages && customMessages[key]) {
        return customMessages[key];
      }

      switch (key) {
        case 'required':
          return 'Bu alan zorunludur.';
        case 'email':
          return 'Geçerli bir e-posta adresi giriniz.';
        case 'minlength':
          return `En az ${errors['minlength'].requiredLength} karakter girmelisiniz.`;
        case 'maxlength':
          return `En fazla ${errors['maxlength'].requiredLength} karakter girebilirsiniz.`;
        case 'min':
          return `Değer en az ${errors['min'].min} olmalıdır.`;
        case 'max':
          return `Değer en fazla ${errors['max'].max} olabilir.`;
        case 'pattern':
          return 'Geçersiz format.';
        case 'taxNumber':
          return 'Geçerli bir vergi numarası (10 haneli) giriniz.';
        case 'identityNumber':
          return 'Geçerli bir T.C. kimlik numarası (11 haneli) giriniz.';
        case 'phone':
          return 'Geçerli bir telefon numarası giriniz.';
        default:
          return 'Geçersiz değer.';
      }
    }

    return '';
  }

  static hasError(control: AbstractControl, errorType: string): boolean {
    return control.hasError(errorType) && (control.dirty || control.touched);
  }
}