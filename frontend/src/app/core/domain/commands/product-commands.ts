import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { ProductRepository } from '../repositories/catalog.repository';
import { CreateProductRequest } from '../models/product.model';
import { ProductValidator } from '../validators/product.validator';
import { ICommand } from './base-command';

@Injectable({ providedIn: 'root' })
export class CreateProductCommand implements ICommand<CreateProductRequest, string> {
  private readonly productRepository = inject(ProductRepository);
  private readonly productValidator = inject(ProductValidator);

  execute(request: CreateProductRequest): Observable<string> {
    return this.productValidator.validateRequest(request).pipe(
      switchMap(() => this.productRepository.create(request))
    );
  }
}

@Injectable({ providedIn: 'root' })
export class UpdateProductCommand implements ICommand<{ id: string; request: CreateProductRequest }, void> {
  private readonly productRepository = inject(ProductRepository);
  private readonly productValidator = inject(ProductValidator);

  execute({ id, request }: { id: string; request: CreateProductRequest }): Observable<void> {
    return this.productValidator.validateRequest(request).pipe(
      switchMap(() => this.productRepository.update(id, request))
    );
  }
}

@Injectable({ providedIn: 'root' })
export class DeleteProductCommand implements ICommand<string, void> {
  private readonly productRepository = inject(ProductRepository);

  execute(id: string): Observable<void> {
    return this.productRepository.delete(id);
  }
}