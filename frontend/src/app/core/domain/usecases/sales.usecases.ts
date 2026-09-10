import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { CustomerRepository, InvoiceRepository } from '../repositories/sales.repository';
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
import { CustomerValidator } from '../validators/customer.validator';

@Injectable({ providedIn: 'root' })
export class GetAllCustomersUseCase {
  private readonly customerRepository = inject(CustomerRepository);

  execute(): Observable<Customer[]> {
    return this.customerRepository.getAll();
  }
}

@Injectable({ providedIn: 'root' })
export class GetCustomersUseCase {
  private readonly customerRepository = inject(CustomerRepository);

  execute(params: PaginationParams): Observable<PaginatedList<Customer>> {
    return this.customerRepository.getPaginated(params);
  }
}

@Injectable({ providedIn: 'root' })
export class GetCustomerByIdUseCase {
  private readonly customerRepository = inject(CustomerRepository);

  execute(id: string): Observable<Customer> {
    return this.customerRepository.getById(id);
  }
}

@Injectable({ providedIn: 'root' })
export class CreateCustomerUseCase {
  private readonly customerRepository = inject(CustomerRepository);
  private readonly customerValidator = inject(CustomerValidator);

  execute(request: CreateCustomerRequest): Observable<string> {
    return this.customerValidator.validateRequest(request).pipe(
      switchMap(() => this.customerRepository.create(request))
    );
  }
}

@Injectable({ providedIn: 'root' })
export class UpdateCustomerUseCase {
  private readonly customerRepository = inject(CustomerRepository);
  private readonly customerValidator = inject(CustomerValidator);

  execute(id: string, request: UpdateCustomerRequest): Observable<void> {
    return this.customerValidator.validateRequest(request).pipe(
      switchMap(() => this.customerRepository.update(id, request))
    );
  }
}

@Injectable({ providedIn: 'root' })
export class DeleteCustomerUseCase {
  private readonly customerRepository = inject(CustomerRepository);

  execute(id: string): Observable<void> {
    return this.customerRepository.delete(id);
  }
}

@Injectable({ providedIn: 'root' })
export class GetAllInvoicesUseCase {
  private readonly invoiceRepository = inject(InvoiceRepository);

  execute(): Observable<Invoice[]> {
    return this.invoiceRepository.getAll();
  }
}

@Injectable({ providedIn: 'root' })
export class GetInvoicesUseCase {
  private readonly invoiceRepository = inject(InvoiceRepository);

  execute(params: PaginationParams): Observable<PaginatedList<Invoice>> {
    return this.invoiceRepository.getPaginated(params);
  }
}

@Injectable({ providedIn: 'root' })
export class GetInvoiceByIdUseCase {
  private readonly invoiceRepository = inject(InvoiceRepository);

  execute(id: string): Observable<Invoice> {
    return this.invoiceRepository.getById(id);
  }
}

@Injectable({ providedIn: 'root' })
export class CreateInvoiceUseCase {
  private readonly invoiceRepository = inject(InvoiceRepository);

  execute(request: CreateInvoiceRequest): Observable<string> {
    return this.invoiceRepository.create(request);
  }
}

@Injectable({ providedIn: 'root' })
export class UpdateInvoiceUseCase {
  private readonly invoiceRepository = inject(InvoiceRepository);

  execute(id: string, request: UpdateInvoiceRequest): Observable<void> {
    return this.invoiceRepository.update(id, request);
  }
}

@Injectable({ providedIn: 'root' })
export class AddPaymentUseCase {
  private readonly invoiceRepository = inject(InvoiceRepository);

  execute(invoiceId: string, request: AddPaymentRequest): Observable<void> {
    return this.invoiceRepository.addPayment(invoiceId, request);
  }
}

@Injectable({ providedIn: 'root' })
export class DeleteInvoiceUseCase {
  private readonly invoiceRepository = inject(InvoiceRepository);

  execute(id: string): Observable<void> {
    return this.invoiceRepository.delete(id);
  }
}

@Injectable({ providedIn: 'root' })
export class DownloadInvoicePdfUseCase {
  private readonly invoiceRepository = inject(InvoiceRepository);

  execute(id: string): Observable<Blob> {
    return this.invoiceRepository.downloadPdf(id);
  }
}