import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';

import { CustomerRepository, InvoiceRepository } from '../../domain/repositories/sales.repository';
import { PaginatedList, PaginationParams } from '../../domain/models/pagination.model';
import {
  AddPaymentRequest,
  CreateCustomerRequest,
  CreateInvoiceRequest,
  Customer,
  Invoice,
  UpdateCustomerRequest,
  UpdateInvoiceRequest,
} from '../../domain/models';
import { ApiBase } from '../api/api-base';

/**
 * CustomerRepository'nin REST API implementasyonu.
 * Backend endpoint'leri: /api/v1/customers
 */
@Injectable({ providedIn: 'root' })
export class CustomerApiRepository extends ApiBase implements CustomerRepository {
  private readonly versionedBase = `${this.baseUrl}/v1`;

  getAll(): Observable<Customer[]> {
    return this.toApiError(this.http.get<Customer[]>(`${this.versionedBase}/customers`));
  }

  getPaginated(params: PaginationParams): Observable<PaginatedList<Customer>> {
    const httpParams = new HttpParams()
      .set('PageNumber', params.pageNumber)
      .set('PageSize', params.pageSize);
    return this.toApiError(
      this.http.get<PaginatedList<Customer>>(`${this.versionedBase}/customers`, { params: httpParams }),
    );
  }

  getById(id: string): Observable<Customer> {
    return this.toApiError(this.http.get<Customer>(`${this.versionedBase}/customers/${id}`));
  }

  create(request: CreateCustomerRequest): Observable<string> {
    return this.toApiError(
      this.http.post<{ customerId: string }>(`${this.versionedBase}/customers`, request),
    ).pipe(map((response) => response.customerId));
  }

  update(id: string, request: UpdateCustomerRequest): Observable<void> {
    return this.toApiError(this.http.put<void>(`${this.versionedBase}/customers/${id}`, request));
  }

  delete(id: string): Observable<void> {
    return this.toApiError(this.http.delete<void>(`${this.versionedBase}/customers/${id}`));
  }
}

/**
 * InvoiceRepository'nin REST API implementasyonu.
 * Backend endpoint'leri: /api/v1/invoices (API versioning kullanır)
 */
@Injectable({ providedIn: 'root' })
export class InvoiceApiRepository extends ApiBase implements InvoiceRepository {
  private readonly versionedBase = `${this.baseUrl}/v1`;

  getAll(): Observable<Invoice[]> {
    return this.toApiError(this.http.get<Invoice[]>(`${this.versionedBase}/invoices`));
  }

  getPaginated(params: PaginationParams): Observable<PaginatedList<Invoice>> {
    const httpParams = new HttpParams()
      .set('PageNumber', params.pageNumber)
      .set('PageSize', params.pageSize);
    return this.toApiError(
      this.http.get<PaginatedList<Invoice>>(`${this.versionedBase}/invoices/paginated`, {
        params: httpParams,
      }),
    );
  }

  getById(id: string): Observable<Invoice> {
    return this.toApiError(this.http.get<Invoice>(`${this.versionedBase}/invoices/${id}`));
  }

  create(request: CreateInvoiceRequest): Observable<string> {
    return this.toApiError(
      this.http.post<{ invoiceId: string }>(`${this.versionedBase}/invoices`, request),
    ).pipe(map((response) => response.invoiceId));
  }

  update(id: string, request: UpdateInvoiceRequest): Observable<void> {
    return this.toApiError(this.http.put<void>(`${this.versionedBase}/invoices/${id}`, request));
  }

  delete(id: string): Observable<void> {
    return this.toApiError(this.http.delete<void>(`${this.versionedBase}/invoices/${id}`));
  }

  addPayment(invoiceId: string, request: AddPaymentRequest): Observable<void> {
    return this.toApiError(
      this.http.post<void>(`${this.versionedBase}/invoices/${invoiceId}/payments`, request),
    );
  }

  downloadPdf(id: string): Observable<Blob> {
    return this.toApiError(
      this.http.get(`${this.versionedBase}/invoices/${id}/pdf`, { responseType: 'blob' }),
    );
  }
}