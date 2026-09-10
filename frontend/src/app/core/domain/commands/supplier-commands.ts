import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { SupplierRepository } from '../repositories/supplier.repository';
import { CreateSupplierRequest } from '../models/supplier.model';
import { ICommand } from './base-command';

@Injectable({ providedIn: 'root' })
export class CreateSupplierCommand implements ICommand<CreateSupplierRequest, string> {
  private readonly supplierRepository = inject(SupplierRepository);

  execute(request: CreateSupplierRequest): Observable<string> {
    return this.supplierRepository.create(request);
  }
}

@Injectable({ providedIn: 'root' })
export class UpdateSupplierCommand implements ICommand<{ id: string; request: CreateSupplierRequest }, void> {
  private readonly supplierRepository = inject(SupplierRepository);

  execute(params: { id: string; request: CreateSupplierRequest }): Observable<void> {
    return this.supplierRepository.update(params.id, params.request);
  }
}

@Injectable({ providedIn: 'root' })
export class DeleteSupplierCommand implements ICommand<string, void> {
  private readonly supplierRepository = inject(SupplierRepository);

  execute(id: string): Observable<void> {
    return this.supplierRepository.delete(id);
  }
}
