import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { SupplierRepository } from '../repositories/supplier.repository';
import { PaginatedList, PaginationParams } from '../models/pagination.model';
import { Supplier } from '../models/supplier.model';
import { IQuery } from './base-query';

@Injectable({ providedIn: 'root' })
export class GetAllSuppliersQuery implements IQuery<void, Supplier[]> {
  private readonly supplierRepository = inject(SupplierRepository);

  execute(): Observable<Supplier[]> {
    return this.supplierRepository.getAll();
  }
}

@Injectable({ providedIn: 'root' })
export class GetPaginatedSuppliersQuery implements IQuery<PaginationParams, PaginatedList<Supplier>> {
  private readonly supplierRepository = inject(SupplierRepository);

  execute(params: PaginationParams): Observable<PaginatedList<Supplier>> {
    return this.supplierRepository.getPaginated(params);
  }
}

@Injectable({ providedIn: 'root' })
export class GetSupplierByIdQuery implements IQuery<string, Supplier> {
  private readonly supplierRepository = inject(SupplierRepository);

  execute(id: string): Observable<Supplier> {
    return this.supplierRepository.getById(id);
  }
}
