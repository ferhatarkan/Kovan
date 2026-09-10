import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';

import { SupplierRepository } from '../../domain/repositories/supplier.repository';
import { PaginatedList, PaginationParams } from '../../domain/models/pagination.model';
import { CreateSupplierRequest, Supplier } from '../../domain/models/supplier.model';
import { ApiBase } from '../api/api-base';

/**
 * SupplierRepository'nin REST API implementasyonu.
 * Backend endpoint'leri: /api/v1/suppliers
 */
@Injectable({ providedIn: 'root' })
export class SupplierApiRepository extends ApiBase implements SupplierRepository {
  private readonly versionedBase = `${this.baseUrl}/v1`;

  getAll(): Observable<Supplier[]> {
    return this.toApiError(this.http.get<Supplier[]>(`${this.versionedBase}/suppliers`));
  }

  getPaginated(params: PaginationParams): Observable<PaginatedList<Supplier>> {
    const httpParams = new HttpParams()
      .set('PageNumber', params.pageNumber)
      .set('PageSize', params.pageSize);
    return this.toApiError(
      this.http.get<PaginatedList<Supplier>>(`${this.versionedBase}/suppliers`, { params: httpParams }),
    );
  }

  getById(id: string): Observable<Supplier> {
    return this.toApiError(this.http.get<Supplier>(`${this.versionedBase}/suppliers/${id}`));
  }

  create(request: CreateSupplierRequest): Observable<string> {
    return this.toApiError(
      this.http.post<{ supplierId: string }>(`${this.versionedBase}/suppliers`, request),
    ).pipe(map((response) => response.supplierId));
  }

  update(id: string, request: CreateSupplierRequest): Observable<void> {
    return this.toApiError(this.http.put<void>(`${this.versionedBase}/suppliers/${id}`, request));
  }

  delete(id: string): Observable<void> {
    return this.toApiError(this.http.delete<void>(`${this.versionedBase}/suppliers/${id}`));
  }
}
