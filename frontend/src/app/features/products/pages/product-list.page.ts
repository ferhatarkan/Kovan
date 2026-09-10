import { Component, inject, signal } from '@angular/core';
import { DecimalPipe } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';

import { Product } from '../../../core/domain/models/product.model';
import { GetProductsUseCase } from '../../../core/domain/usecases/catalog.usecases';
import { ApiError } from '../../../core/infrastructure/api/api-error';

@Component({
  selector: 'app-product-list',
  imports: [DecimalPipe],
  templateUrl: './product-list.page.html',
  styleUrl: './product-list.page.scss',
})
export default class ProductListPage {
  private readonly getProducts = inject(GetProductsUseCase);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);

  readonly products = signal<Product[]>([]);
  readonly totalCount = signal(0);
  readonly pageNumber = signal(1);
  readonly totalPages = signal(1);
  readonly loading = signal(false);
  readonly errorMessage = signal<string | null>(null);
  readonly pageSize = 10;

  constructor() {
    this.loadProducts();
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
    this.loadProducts(page);
  }

  openCreate(): void {
    void this.router.navigate(['new'], { relativeTo: this.route });
  }

  private loadProducts(page = 1): void {
    this.loading.set(true);
    this.errorMessage.set(null);

    this.getProducts.execute({ pageNumber: page, pageSize: this.pageSize }).subscribe({
      next: (result) => {
        this.products.set(result.items);
        this.totalCount.set(result.totalCount);
        this.pageNumber.set(result.pageNumber);
        this.totalPages.set(result.totalPages);
        this.loading.set(false);
      },
      error: (error: unknown) => {
        this.loading.set(false);
        this.errorMessage.set(
          error instanceof ApiError ? error.message : 'Ürünler yüklenemedi.',
        );
      },
    });
  }
}