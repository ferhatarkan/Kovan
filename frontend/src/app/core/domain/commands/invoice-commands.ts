import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { InvoiceRepository } from '../repositories/sales.repository';
import { AddPaymentRequest, CreateInvoiceRequest, UpdateInvoiceRequest } from '../models/invoice.model';
import { ICommand } from './base-command';

@Injectable({ providedIn: 'root' })
export class CreateInvoiceCommand implements ICommand<CreateInvoiceRequest, string> {
  private readonly invoiceRepository = inject(InvoiceRepository);

  execute(request: CreateInvoiceRequest): Observable<string> {
    return this.invoiceRepository.create(request);
  }
}

@Injectable({ providedIn: 'root' })
export class UpdateInvoiceCommand implements ICommand<{ id: string; request: UpdateInvoiceRequest }, void> {
  private readonly invoiceRepository = inject(InvoiceRepository);

  execute(params: { id: string; request: UpdateInvoiceRequest }): Observable<void> {
    return this.invoiceRepository.update(params.id, params.request);
  }
}

@Injectable({ providedIn: 'root' })
export class AddPaymentCommand implements ICommand<{ invoiceId: string; request: AddPaymentRequest }, void> {
  private readonly invoiceRepository = inject(InvoiceRepository);

  execute({ invoiceId, request }: { invoiceId: string; request: AddPaymentRequest }): Observable<void> {
    return this.invoiceRepository.addPayment(invoiceId, request);
  }
}

@Injectable({ providedIn: 'root' })
export class DeleteInvoiceCommand implements ICommand<string, void> {
  private readonly invoiceRepository = inject(InvoiceRepository);

  execute(id: string): Observable<void> {
    return this.invoiceRepository.delete(id);
  }
}