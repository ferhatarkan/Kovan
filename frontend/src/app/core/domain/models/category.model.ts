/**
 * Backend'deki Kovan.Application.Features.Categories modellerinin karşılığıdır.
 */

export interface Category {
  id: string;
  name: string;
}

export interface CreateCategoryRequest {
  name: string;
}

export type UpdateCategoryRequest = CreateCategoryRequest;