import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { InvoiceRepository } from '../repositories/sales.repository';
import { PaginatedList, PaginationParams } from '../models/pagination.model';
import { Invoice } from '../models/invoice.model';
import { IQuery } from './base-query';

@Injectable({ providedIn: 'root' })
export class GetAllInvoicesQuery implements IQuery<void, Invoice[]> {
  private readonly invoiceRepository = inject(InvoiceRepository);

  execute(): Observable<Invoice[]> {
    return this.invoiceRepository.getAll();
  }
}

@Injectable({ providedIn: 'root' })
export class GetPaginatedInvoicesQuery implements IQuery<PaginationParams, PaginatedList<Invoice>> {
  private readonly invoiceRepository = inject(InvoiceRepository);

  execute(params: PaginationParams): Observable<PaginatedList<Invoice>> {
    return this.invoiceRepository.getPaginated(params);
  }
}

@Injectable({ providedIn: 'root' })
export class GetInvoiceByIdQuery implements IQuery<string, Invoice> {
  private readonly invoiceRepository = inject(InvoiceRepository);

  execute(id: string): Observable<Invoice> {
    return this.invoiceRepository.getById(id);
  }
}

@Injectable({ providedIn: 'root' })
export class GetInvoicePdfQuery implements IQuery<string, Blob> {
  private readonly invoiceRepository = inject(InvoiceRepository);

  execute(id: string): Observable<Blob> {
    return this.invoiceRepository.downloadPdf(id);
  }
}