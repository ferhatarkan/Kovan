import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { CategoryRepository } from '../repositories/catalog.repository';
import { CreateCategoryRequest } from '../models/category.model';
import { ICommand } from './base-command';

@Injectable({ providedIn: 'root' })
export class CreateCategoryCommand implements ICommand<CreateCategoryRequest, string> {
  private readonly categoryRepository = inject(CategoryRepository);

  execute(request: CreateCategoryRequest): Observable<string> {
    return this.categoryRepository.create(request);
  }
}

@Injectable({ providedIn: 'root' })
export class UpdateCategoryCommand implements ICommand<{ id: string; request: CreateCategoryRequest }, void> {
  private readonly categoryRepository = inject(CategoryRepository);

  execute(params: { id: string; request: CreateCategoryRequest }): Observable<void> {
    return this.categoryRepository.update(params.id, params.request);
  }
}

@Injectable({ providedIn: 'root' })
export class DeleteCategoryCommand implements ICommand<string, void> {
  private readonly categoryRepository = inject(CategoryRepository);

  execute(id: string): Observable<void> {
    return this.categoryRepository.delete(id);
  }
}
