import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { SupplierRepository } from '../repositories/supplier.repository';
import { PaginatedList, PaginationParams } from '../models/pagination.model';
import { CreateSupplierRequest, Supplier } from '../models/supplier.model';

@Injectable({ providedIn: 'root' })
export class GetAllSuppliersUseCase {
  private readonly supplierRepository = inject(SupplierRepository);

  execute(): Observable<Supplier[]> {
    return this.supplierRepository.getAll();
  }
}

@Injectable({ providedIn: 'root' })
export class GetPaginatedSuppliersUseCase {
  private readonly supplierRepository = inject(SupplierRepository);

  execute(params: PaginationParams): Observable<PaginatedList<Supplier>> {
    return this.supplierRepository.getPaginated(params);
  }
}

@Injectable({ providedIn: 'root' })
export class GetSupplierByIdUseCase {
  private readonly supplierRepository = inject(SupplierRepository);

  execute(id: string): Observable<Supplier> {
    return this.supplierRepository.getById(id);
  }
}

@Injectable({ providedIn: 'root' })
export class CreateSupplierUseCase {
  private readonly supplierRepository = inject(SupplierRepository);

  execute(request: CreateSupplierRequest): Observable<string> {
    return this.supplierRepository.create(request);
  }
}

@Injectable({ providedIn: 'root' })
export class UpdateSupplierUseCase {
  private readonly supplierRepository = inject(SupplierRepository);

  execute(id: string, request: CreateSupplierRequest): Observable<void> {
    return this.supplierRepository.update(id, request);
  }
}

@Injectable({ providedIn: 'root' })
export class DeleteSupplierUseCase {
  private readonly supplierRepository = inject(SupplierRepository);

  execute(id: string): Observable<void> {
    return this.supplierRepository.delete(id);
  }
}
