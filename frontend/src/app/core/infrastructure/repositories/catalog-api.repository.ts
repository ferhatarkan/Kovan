import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';

import { CategoryRepository, ProductRepository } from '../../domain/repositories/catalog.repository';
import { PaginatedList, PaginationParams } from '../../domain/models/pagination.model';
import { Category, CreateCategoryRequest } from '../../domain/models/category.model';
import { CreateProductRequest, Product } from '../../domain/models/product.model';
import { ApiBase } from '../api/api-base';

/**
 * ProductRepository'nin REST API implementasyonu.
 * Backend endpoint'leri: /api/v1/products
 */
@Injectable({ providedIn: 'root' })
export class ProductApiRepository extends ApiBase implements ProductRepository {
  private readonly versionedBase = `${this.baseUrl}/v1`;

  getAll(): Observable<Product[]> {
    return this.toApiError(this.http.get<Product[]>(`${this.versionedBase}/products`));
  }

  getPaginated(params: PaginationParams): Observable<PaginatedList<Product>> {
    const httpParams = new HttpParams()
      .set('PageNumber', params.pageNumber)
      .set('PageSize', params.pageSize);
    return this.toApiError(
      this.http.get<PaginatedList<Product>>(`${this.versionedBase}/products`, { params: httpParams }),
    );
  }

  getById(id: string): Observable<Product> {
    return this.toApiError(this.http.get<Product>(`${this.versionedBase}/products/${id}`));
  }

  create(request: CreateProductRequest): Observable<string> {
    return this.toApiError(
      this.http.post<{ productId: string }>(`${this.versionedBase}/products`, request),
    ).pipe(map((response) => response.productId));
  }

  downloadLabel(id: string): Observable<Blob> {
    return this.toApiError(
      this.http.get(`${this.versionedBase}/products/${id}/label`, { responseType: 'blob' }),
    );
  }

  update(id: string, request: CreateProductRequest): Observable<void> {
    return this.toApiError(this.http.put<void>(`${this.versionedBase}/products/${id}`, request));
  }

  delete(id: string): Observable<void> {
    return this.toApiError(this.http.delete<void>(`${this.versionedBase}/products/${id}`));
  }
}

/**
 * CategoryRepository'nin REST API implementasyonu.
 * Backend endpoint'leri: /api/v1/categories
 */
@Injectable({ providedIn: 'root' })
export class CategoryApiRepository extends ApiBase implements CategoryRepository {
  private readonly versionedBase = `${this.baseUrl}/v1`;

  getAll(): Observable<Category[]> {
    return this.toApiError(this.http.get<Category[]>(`${this.versionedBase}/categories`));
  }

  getPaginated(params: PaginationParams): Observable<PaginatedList<Category>> {
    const httpParams = new HttpParams()
      .set('PageNumber', params.pageNumber)
      .set('PageSize', params.pageSize);
    return this.toApiError(
      this.http.get<PaginatedList<Category>>(`${this.versionedBase}/categories/paginated`, { params: httpParams }),
    );
  }

  getById(id: string): Observable<Category> {
    return this.toApiError(this.http.get<Category>(`${this.versionedBase}/categories/${id}`));
  }

  create(request: CreateCategoryRequest): Observable<string> {
    return this.toApiError(
      this.http.post<{ categoryId: string }>(`${this.versionedBase}/categories`, request),
    ).pipe(map((response) => response.categoryId));
  }

  update(id: string, request: CreateCategoryRequest): Observable<void> {
    return this.toApiError(this.http.put<void>(`${this.versionedBase}/categories/${id}`, request));
  }

  delete(id: string): Observable<void> {
    return this.toApiError(this.http.delete<void>(`${this.versionedBase}/categories/${id}`));
  }
}