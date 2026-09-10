import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { CustomerRepository } from '../repositories/sales.repository';
import { PaginatedList, PaginationParams } from '../models/pagination.model';
import { Customer } from '../models/customer.model';
import { IQuery } from './base-query';

@Injectable({ providedIn: 'root' })
export class GetAllCustomersQuery implements IQuery<void, Customer[]> {
  private readonly customerRepository = inject(CustomerRepository);

  execute(): Observable<Customer[]> {
    return this.customerRepository.getAll();
  }
}

@Injectable({ providedIn: 'root' })
export class GetPaginatedCustomersQuery implements IQuery<PaginationParams, PaginatedList<Customer>> {
  private readonly customerRepository = inject(CustomerRepository);

  execute(params: PaginationParams): Observable<PaginatedList<Customer>> {
    return this.customerRepository.getPaginated(params);
  }
}

@Injectable({ providedIn: 'root' })
export class GetCustomerByIdQuery implements IQuery<string, Customer> {
  private readonly customerRepository = inject(CustomerRepository);

  execute(id: string): Observable<Customer> {
    return this.customerRepository.getById(id);
  }
}