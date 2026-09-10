import { Observable } from 'rxjs';

import { PaginatedList, PaginationParams } from '../models/pagination.model';
import { CreateSupplierRequest, Supplier } from '../models/supplier.model';

/**
 * Tedarikçi işlemleri için domain port'u.
 */
export abstract class SupplierRepository {
  abstract getAll(): Observable<Supplier[]>;
  abstract getPaginated(params: PaginationParams): Observable<PaginatedList<Supplier>>;
  abstract getById(id: string): Observable<Supplier>;
  abstract create(request: CreateSupplierRequest): Observable<string>;
  abstract update(id: string, request: CreateSupplierRequest): Observable<void>;
  abstract delete(id: string): Observable<void>;
}
