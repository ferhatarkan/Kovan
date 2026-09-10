import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { CategoryRepository, ProductRepository } from '../repositories/catalog.repository';
import { PaginatedList, PaginationParams } from '../models/pagination.model';
import { Category, CreateCategoryRequest } from '../models/category.model';
import { CreateProductRequest, Product } from '../models/product.model';
import { ProductValidator } from '../validators/product.validator';

// Queries (Read operations)
@Injectable({ providedIn: 'root' })
export class GetAllProductsUseCase {
  private readonly productRepository = inject(ProductRepository);

  execute(): Observable<Product[]> {
    return this.productRepository.getAll();
  }
}

@Injectable({ providedIn: 'root' })
export class GetProductsUseCase {
  private readonly productRepository = inject(ProductRepository);

  execute(params: PaginationParams): Observable<PaginatedList<Product>> {
    return this.productRepository.getPaginated(params);
  }
}

@Injectable({ providedIn: 'root' })
export class GetProductByIdUseCase {
  private readonly productRepository = inject(ProductRepository);

  execute(id: string): Observable<Product> {
    return this.productRepository.getById(id);
  }
}

@Injectable({ providedIn: 'root' })
export class DownloadProductLabelUseCase {
  private readonly productRepository = inject(ProductRepository);

  execute(id: string): Observable<Blob> {
    return this.productRepository.downloadLabel(id);
  }
}

// Commands (Write operations)
@Injectable({ providedIn: 'root' })
export class CreateProductUseCase {
  private readonly productRepository = inject(ProductRepository);
  private readonly productValidator = inject(ProductValidator);

  execute(request: CreateProductRequest): Observable<string> {
    return this.productValidator.validateRequest(request).pipe(
      switchMap(() => this.productRepository.create(request))
    );
  }
}

@Injectable({ providedIn: 'root' })
export class UpdateProductUseCase {
  private readonly productRepository = inject(ProductRepository);
  private readonly productValidator = inject(ProductValidator);

  execute(id: string, request: CreateProductRequest): Observable<void> {
    return this.productValidator.validateRequest(request).pipe(
      switchMap(() => this.productRepository.update(id, request))
    );
  }
}

@Injectable({ providedIn: 'root' })
export class DeleteProductUseCase {
  private readonly productRepository = inject(ProductRepository);

  execute(id: string): Observable<void> {
    return this.productRepository.delete(id);
  }
}

// Legacy use cases for backward compatibility
@Injectable({ providedIn: 'root' })
export class GetCategoriesUseCase {
  private readonly categoryRepository = inject(CategoryRepository);

  execute(): Observable<Category[]> {
    return this.categoryRepository.getAll();
  }
}

@Injectable({ providedIn: 'root' })
export class GetPaginatedCategoriesUseCase {
  private readonly categoryRepository = inject(CategoryRepository);

  execute(params: PaginationParams): Observable<PaginatedList<Category>> {
    return this.categoryRepository.getPaginated(params);
  }
}

@Injectable({ providedIn: 'root' })
export class GetCategoryByIdUseCase {
  private readonly categoryRepository = inject(CategoryRepository);

  execute(id: string): Observable<Category> {
    return this.categoryRepository.getById(id);
  }
}

@Injectable({ providedIn: 'root' })
export class CreateCategoryUseCase {
  private readonly categoryRepository = inject(CategoryRepository);

  execute(request: CreateCategoryRequest): Observable<string> {
    return this.categoryRepository.create(request);
  }
}

@Injectable({ providedIn: 'root' })
export class UpdateCategoryUseCase {
  private readonly categoryRepository = inject(CategoryRepository);

  execute(id: string, request: CreateCategoryRequest): Observable<void> {
    return this.categoryRepository.update(id, request);
  }
}

@Injectable({ providedIn: 'root' })
export class DeleteCategoryUseCase {
  private readonly categoryRepository = inject(CategoryRepository);

  execute(id: string): Observable<void> {
    return this.categoryRepository.delete(id);
  }
}