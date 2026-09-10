import { Component, inject, signal } from '@angular/core';
import { DatePipe, DecimalPipe } from '@angular/common';

import { Invoice, InvoiceStatus } from '../../../core/domain/models/invoice.model';
import { GetInvoicesUseCase } from '../../../core/domain/usecases/sales.usecases';
import { ApiError } from '../../../core/infrastructure/api/api-error';

const STATUS_LABELS: Record<InvoiceStatus, string> = {
  [InvoiceStatus.Draft]: 'Taslak',
  [InvoiceStatus.Sent]: 'Gönderildi',
  [InvoiceStatus.PartiallyPaid]: 'Kısmi Ödendi',
  [InvoiceStatus.Paid]: 'Ödendi',
  [InvoiceStatus.Overdue]: 'Vadesi Geçti',
};

const STATUS_CSS_CLASSES: Record<InvoiceStatus, string> = {
  [InvoiceStatus.Draft]: 'status-draft',
  [InvoiceStatus.Sent]: 'status-sent',
  [InvoiceStatus.PartiallyPaid]: 'status-partially-paid',
  [InvoiceStatus.Paid]: 'status-paid',
  [InvoiceStatus.Overdue]: 'status-overdue',
};

@Component({
  selector: 'app-invoice-list',
  imports: [DatePipe, DecimalPipe],
  templateUrl: './invoice-list.page.html',
  styleUrl: './invoice-list.page.scss',
})
export default class InvoiceListPage {
  private readonly getInvoices = inject(GetInvoicesUseCase);

  readonly invoices = signal<Invoice[]>([]);
  readonly totalCount = signal(0);
  readonly pageNumber = signal(1);
  readonly totalPages = signal(1);
  readonly loading = signal(false);
  readonly errorMessage = signal<string | null>(null);
  readonly statusLabels = STATUS_LABELS;
  readonly statusCssClasses = STATUS_CSS_CLASSES;
  readonly invoiceStatus = InvoiceStatus;
  readonly pageSize = 10;

  constructor() {
    this.loadInvoices();
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
    this.loadInvoices(page);
  }

  private loadInvoices(page = 1): void {
    this.loading.set(true);
    this.errorMessage.set(null);

    this.getInvoices.execute({ pageNumber: page, pageSize: this.pageSize }).subscribe({
      next: (result) => {
        this.invoices.set(result.items);
        this.totalCount.set(result.totalCount);
        this.pageNumber.set(result.pageNumber);
        this.totalPages.set(result.totalPages);
        this.loading.set(false);
      },
      error: (error: unknown) => {
        this.loading.set(false);
        this.errorMessage.set(
          error instanceof ApiError ? error.message : 'Faturalar yüklenemedi.',
        );
      },
    });
  }
}