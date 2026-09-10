import { Injectable } from '@angular/core';
import { BaseValidator, ValidationResult } from './base-validator';
import { CreateProductRequest } from '../models/product.model';

@Injectable({ providedIn: 'root' })
export class ProductValidator extends BaseValidator<CreateProductRequest> {
  validate(request: CreateProductRequest): ValidationResult {
    const errors: Record<string, string> = {};

    if (!request.name || request.name.trim().length === 0) {
      errors['name'] = 'Ürün adı zorunludur.';
    }

    if (!request.sku || request.sku.trim().length === 0) {
      errors['sku'] = 'Ürün kodu (SKU) zorunludur.';
    }

    if (!request.brand || request.brand.trim().length === 0) {
      errors['brand'] = 'Marka adı zorunludur.';
    }

    if (!request.categoryId || request.categoryId.trim().length === 0) {
      errors['categoryId'] = 'Kategori seçimi zorunludur.';
    }

    if (request.price === null || request.price === undefined || request.price < 0) {
      errors['price'] = 'Fiyat negatif olamaz.';
    }

    return {
      isValid: Object.keys(errors).length === 0,
      errors,
    };
  }
}