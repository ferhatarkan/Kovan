/**
 * Backend'deki Kovan.Application.Features.Products modellerinin karşılığıdır.
 */

export interface Product {
  id: string;
  name: string;
  sku: string;
  price: number;
  categoryId: string;
  categoryName: string;
  brand: string;
  stockQuantity: number;
}

export interface CreateProductRequest {
  name: string;
  sku: string;
  price: number;
  categoryId: string;
  brand: string;
  properties?: Record<string, string> | null;
}

export type UpdateProductRequest = CreateProductRequest;