import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { ProductRepository } from '../repositories/catalog.repository';
import { PaginatedList, PaginationParams } from '../models/pagination.model';
import { Product } from '../models/product.model';
import { IQuery } from './base-query';

@Injectable({ providedIn: 'root' })
export class GetAllProductsQuery implements IQuery<void, Product[]> {
  private readonly productRepository = inject(ProductRepository);

  execute(): Observable<Product[]> {
    return this.productRepository.getAll();
  }
}

@Injectable({ providedIn: 'root' })
export class GetPaginatedProductsQuery implements IQuery<PaginationParams, PaginatedList<Product>> {
  private readonly productRepository = inject(ProductRepository);

  execute(params: PaginationParams): Observable<PaginatedList<Product>> {
    return this.productRepository.getPaginated(params);
  }
}

@Injectable({ providedIn: 'root' })
export class GetProductByIdQuery implements IQuery<string, Product> {
  private readonly productRepository = inject(ProductRepository);

  execute(id: string): Observable<Product> {
    return this.productRepository.getById(id);
  }
}

@Injectable({ providedIn: 'root' })
export class GetProductLabelQuery implements IQuery<string, Blob> {
  private readonly productRepository = inject(ProductRepository);

  execute(id: string): Observable<Blob> {
    return this.productRepository.downloadLabel(id);
  }
}