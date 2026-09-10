import { Injectable } from '@angular/core';
import { BaseValidator, ValidationResult } from './base-validator';
import { CreateCustomerRequest, CustomerType } from '../models/customer.model';

@Injectable({ providedIn: 'root' })
export class CustomerValidator extends BaseValidator<CreateCustomerRequest> {
  validate(request: CreateCustomerRequest): ValidationResult {
    const errors: Record<string, string> = {};

    // Common validations
    if (!request.phoneNumber || request.phoneNumber.trim().length === 0) {
      errors['phoneNumber'] = 'Telefon numarası zorunludur.';
    }

    if (!request.address || request.address.trim().length === 0) {
      errors['address'] = 'Adres zorunludur.';
    }

    // Customer type specific validations
    if (request.customerType === CustomerType.Individual) {
      if (!request.firstName || request.firstName.trim().length === 0) {
        errors['firstName'] = 'Ad zorunludur.';
      }
      if (!request.lastName || request.lastName.trim().length === 0) {
        errors['lastName'] = 'Soyad zorunludur.';
      }
      if (!request.nationalIdentityNumber || request.nationalIdentityNumber.trim().length === 0) {
        errors['nationalIdentityNumber'] = 'T.C. kimlik numarası zorunludur.';
      } else if (!/^[0-9]{11}$/.test(request.nationalIdentityNumber)) {
        errors['nationalIdentityNumber'] = 'Geçerli bir T.C. kimlik numarası (11 haneli) giriniz.';
      }
    } else if (request.customerType === CustomerType.Corporate) {
      if (!request.title || request.title.trim().length === 0) {
        errors['title'] = 'Müşteri ünvanı zorunludur.';
      }
      if (!request.taxNumber || request.taxNumber.trim().length === 0) {
        errors['taxNumber'] = 'Vergi numarası zorunludur.';
      } else if (!/^[0-9]{10}$/.test(request.taxNumber)) {
        errors['taxNumber'] = 'Geçerli bir vergi numarası (10 haneli) giriniz.';
      }
      if (!request.taxOffice || request.taxOffice.trim().length === 0) {
        errors['taxOffice'] = 'Vergi dairesi zorunludur.';
      }
    }

    // Email validation
    if (request.email && request.email.trim().length > 0) {
      const emailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
      if (!emailPattern.test(request.email)) {
        errors['email'] = 'Geçerli bir e-posta adresi giriniz.';
      }
    }

    return {
      isValid: Object.keys(errors).length === 0,
      errors,
    };
  }
}