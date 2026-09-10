import { Observable } from 'rxjs';

import { PaginatedList, PaginationParams } from '../models/pagination.model';
import {
  AddPaymentRequest,
  CreateCustomerRequest,
  CreateInvoiceRequest,
  Customer,
  Invoice,
  UpdateCustomerRequest,
  UpdateInvoiceRequest,
} from '../models';

/**
 * Müşteri ve fatura işlemleri için domain port'u.
 */
export abstract class CustomerRepository {
  abstract getAll(): Observable<Customer[]>;
  abstract getPaginated(params: PaginationParams): Observable<PaginatedList<Customer>>;
  abstract getById(id: string): Observable<Customer>;
  abstract create(request: CreateCustomerRequest): Observable<string>;
  abstract update(id: string, request: UpdateCustomerRequest): Observable<void>;
  abstract delete(id: string): Observable<void>;
}

export abstract class InvoiceRepository {
  abstract getAll(): Observable<Invoice[]>;
  abstract getPaginated(params: PaginationParams): Observable<PaginatedList<Invoice>>;
  abstract getById(id: string): Observable<Invoice>;
  abstract create(request: CreateInvoiceRequest): Observable<string>;
  abstract update(id: string, request: UpdateInvoiceRequest): Observable<void>;
  abstract delete(id: string): Observable<void>;
  abstract addPayment(invoiceId: string, request: AddPaymentRequest): Observable<void>;
  abstract downloadPdf(id: string): Observable<Blob>;
}