import { Component, inject, signal } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { Customer, CustomerType } from '../../../core/domain/models/customer.model';
import {
  GetCustomersUseCase,
  DeleteCustomerUseCase,
} from '../../../core/domain/usecases/sales.usecases';
import { ApiError } from '../../../core/infrastructure/api/api-error';

@Component({
  selector: 'app-customer-list',
  imports: [],
  templateUrl: './customer-list.page.html',
  styleUrl: './customer-list.page.scss',
})
export default class CustomerListPage {
  private readonly getCustomers = inject(GetCustomersUseCase);
  private readonly deleteCustomerUseCase = inject(DeleteCustomerUseCase);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);

  readonly customers = signal<Customer[]>([]);
  readonly totalCount = signal(0);
  readonly pageNumber = signal(1);
  readonly totalPages = signal(1);
  readonly loading = signal(false);
  readonly errorMessage = signal<string | null>(null);
  readonly customerType = CustomerType;
  readonly pageSize = 10;
  readonly deletingCustomerId = signal<string | null>(null);

  constructor() {
    this.loadCustomers();
  }

  hasPrevious(): boolean {
    return this.pageNumber() > 1;
  }

  hasNext(): boolean {
    return this.pageNumber() < this.totalPages();
  }

  goToPage(page: number): void {
    if (page < 1 || page > this.totalPages() || page === this.pageNumber()) {
      return;
    }
    this.loadCustomers(page);
  }

  typeLabel(type: CustomerType): string {
    return type === CustomerType.Corporate ? 'Kurumsal' : 'Bireysel';
  }

  openCreate(): void {
    void this.router.navigate(['new'], { relativeTo: this.route });
  }

  openEdit(customerId: string): void {
    void this.router.navigate([customerId], { relativeTo: this.route });
  }

  deleteCustomer(customerId: string): void {
    if (!confirm('Bu müşteriyi silmek istediğinizden emin misiniz?')) {
      return;
    }

    this.deletingCustomerId.set(customerId);
    this.deleteCustomerUseCase.execute(customerId).subscribe({
      next: () => {
        this.deletingCustomerId.set(null);
        this.loadCustomers(this.pageNumber());
      },
      error: (error: unknown) => {
        this.deletingCustomerId.set(null);
        this.errorMessage.set(error instanceof ApiError ? error.message : 'Müşteri silinemedi.');
      },
    });
  }

  private loadCustomers(page = 1): void {
    this.loading.set(true);
    this.errorMessage.set(null);

    this.getCustomers.execute({ pageNumber: page, pageSize: this.pageSize }).subscribe({
      next: (result) => {
        this.customers.set(result.items);
        this.totalCount.set(result.totalCount);
        this.pageNumber.set(result.pageNumber);
        this.totalPages.set(result.totalPages);
        this.loading.set(false);
      },
      error: (error: unknown) => {
        this.loading.set(false);
        this.errorMessage.set(
          error instanceof ApiError ? error.message : 'Müşteriler yüklenemedi.',
        );
      },
    });
  }
}
