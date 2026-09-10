import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { CategoryRepository } from '../repositories/catalog.repository';
import { PaginatedList, PaginationParams } from '../models/pagination.model';
import { Category } from '../models/category.model';
import { IQuery } from './base-query';

@Injectable({ providedIn: 'root' })
export class GetAllCategoriesQuery implements IQuery<void, Category[]> {
  private readonly categoryRepository = inject(CategoryRepository);

  execute(): Observable<Category[]> {
    return this.categoryRepository.getAll();
  }
}

@Injectable({ providedIn: 'root' })
export class GetPaginatedCategoriesQuery implements IQuery<PaginationParams, PaginatedList<Category>> {
  private readonly categoryRepository = inject(CategoryRepository);

  execute(params: PaginationParams): Observable<PaginatedList<Category>> {
    return this.categoryRepository.getPaginated(params);
  }
}

@Injectable({ providedIn: 'root' })
export class GetCategoryByIdQuery implements IQuery<string, Category> {
  private readonly categoryRepository = inject(CategoryRepository);

  execute(id: string): Observable<Category> {
    return this.categoryRepository.getById(id);
  }
}
