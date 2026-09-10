import { Observable } from 'rxjs';

import { PaginatedList, PaginationParams } from '../models/pagination.model';
import { Category, CreateCategoryRequest } from '../models/category.model';
import { CreateProductRequest, Product } from '../models/product.model';

/**
 * Ürün ve kategori işlemleri için domain port'u.
 */
export abstract class ProductRepository {
  abstract getAll(): Observable<Product[]>;
  abstract getPaginated(params: PaginationParams): Observable<PaginatedList<Product>>;
  abstract getById(id: string): Observable<Product>;
  abstract create(request: CreateProductRequest): Observable<string>;
  abstract update(id: string, request: CreateProductRequest): Observable<void>;
  abstract delete(id: string): Observable<void>;
  abstract downloadLabel(id: string): Observable<Blob>;
}

export abstract class CategoryRepository {
  abstract getAll(): Observable<Category[]>;
  abstract getPaginated(params: PaginationParams): Observable<PaginatedList<Category>>;
  abstract getById(id: string): Observable<Category>;
  abstract create(request: CreateCategoryRequest): Observable<string>;
  abstract update(id: string, request: CreateCategoryRequest): Observable<void>;
  abstract delete(id: string): Observable<void>;
}