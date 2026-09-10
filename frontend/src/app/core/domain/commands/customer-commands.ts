import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { CustomerRepository } from '../repositories/sales.repository';
import { CreateCustomerRequest, UpdateCustomerRequest } from '../models/customer.model';
import { CustomerValidator } from '../validators/customer.validator';
import { ICommand } from './base-command';

@Injectable({ providedIn: 'root' })
export class CreateCustomerCommand implements ICommand<CreateCustomerRequest, string> {
  private readonly customerRepository = inject(CustomerRepository);
  private readonly customerValidator = inject(CustomerValidator);

  execute(request: CreateCustomerRequest): Observable<string> {
    return this.customerValidator.validateRequest(request).pipe(
      switchMap(() => this.customerRepository.create(request))
    );
  }
}

@Injectable({ providedIn: 'root' })
export class UpdateCustomerCommand implements ICommand<{ id: string; request: UpdateCustomerRequest }, void> {
  private readonly customerRepository = inject(CustomerRepository);
  private readonly customerValidator = inject(CustomerValidator);

  execute({ id, request }: { id: string; request: UpdateCustomerRequest }): Observable<void> {
    return this.customerValidator.validateRequest(request).pipe(
      switchMap(() => this.customerRepository.update(id, request))
    );
  }
}

@Injectable({ providedIn: 'root' })
export class DeleteCustomerCommand implements ICommand<string, void> {
  private readonly customerRepository = inject(CustomerRepository);

  execute(id: string): Observable<void> {
    return this.customerRepository.delete(id);
  }
}