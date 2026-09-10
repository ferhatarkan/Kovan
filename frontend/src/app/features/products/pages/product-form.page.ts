import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { Product, CreateProductRequest } from '../../../core/domain/models/product.model';
import { Category } from '../../../core/domain/models/category.model';
import { ProductRepository } from '../../../core/domain/repositories/catalog.repository';
import { CategoryRepository } from '../../../core/domain/repositories/catalog.repository';
import { ApiError } from '../../../core/infrastructure/api/api-error';

@Component({
  selector: 'app-product-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './product-form.page.html',
  styleUrl: './product-form.page.scss',
})
export default class ProductFormPage {
  private readonly fb = inject(FormBuilder);
  private readonly productRepository = inject(ProductRepository);
  private readonly categoryRepository = inject(CategoryRepository);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);

  readonly form: FormGroup;
  readonly productId = signal<string | null>(null);
  readonly loading = signal(false);
  readonly errorMessage = signal<string | null>(null);
  readonly isEditMode = signal(false);
  readonly categories = signal<Category[]>([]);
  readonly loadingCategories = signal(false);

  constructor() {
    this.form = this.fb.group({
      name: ['', Validators.required],
      sku: ['', Validators.required],
      price: [0, [Validators.required, Validators.min(0)]],
      categoryId: ['', Validators.required],
      brand: ['', Validators.required],
      properties: [''],
    });

    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.productId.set(id);
      this.isEditMode.set(true);
      this.loadProduct(id);
    }

    this.loadCategories();
  }

  private loadCategories(): void {
    this.loadingCategories.set(true);
    this.categoryRepository.getAll().subscribe({
      next: (categories) => {
        this.categories.set(categories);
        this.loadingCategories.set(false);
      },
      error: (error: unknown) => {
        this.loadingCategories.set(false);
        this.errorMessage.set(
          error instanceof ApiError ? error.message : 'Kategoriler yüklenemedi.',
        );
      },
    });
  }

  private loadProduct(id: string): void {
    this.loading.set(true);
    this.productRepository.getById(id).subscribe({
      next: (product) => {
        this.form.patchValue({
          name: product.name,
          sku: product.sku,
          price: product.price,
          categoryId: product.categoryId,
          brand: product.brand,
          properties: '',
        });
        this.loading.set(false);
      },
      error: (error: unknown) => {
        this.loading.set(false);
        this.errorMessage.set(
          error instanceof ApiError ? error.message : 'Ürün bilgileri yüklenemedi.',
        );
      },
    });
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.loading.set(true);
    this.errorMessage.set(null);

    const formValue = this.form.value;
    let properties: Record<string, string> | null = null;

    try {
      const parsedProperties: unknown = formValue.properties
        ? JSON.parse(formValue.properties)
        : null;

      if (
        parsedProperties !== null &&
        (typeof parsedProperties !== 'object' ||
          Array.isArray(parsedProperties) ||
          Object.values(parsedProperties).some((value) => typeof value !== 'string'))
      ) {
        throw new Error('Invalid properties');
      }

      properties = parsedProperties as Record<string, string> | null;
    } catch {
      this.loading.set(false);
      this.errorMessage.set('Özellikler geçerli bir JSON olmalıdır.');
      return;
    }

    const createRequest: CreateProductRequest = {
      name: formValue.name,
      sku: formValue.sku,
      price: formValue.price,
      categoryId: formValue.categoryId,
      brand: formValue.brand,
      properties,
    };

    if (this.isEditMode()) {
      this.productRepository.update(this.productId()!, createRequest).subscribe({
        next: () => {
          this.loading.set(false);
          void this.router.navigate(['../'], { relativeTo: this.route });
        },
        error: (error: unknown) => {
          this.loading.set(false);
          this.errorMessage.set(error instanceof ApiError ? error.message : 'Ürün güncellenemedi.');
        },
      });
    } else {
      this.productRepository.create(createRequest).subscribe({
        next: () => {
          this.loading.set(false);
          void this.router.navigate(['../'], { relativeTo: this.route });
        },
        error: (error: unknown) => {
          this.loading.set(false);
          this.errorMessage.set(error instanceof ApiError ? error.message : 'Ürün oluşturulamadı.');
        },
      });
    }
  }

  onCancel(): void {
    void this.router.navigate(['../'], { relativeTo: this.route });
  }

  get pageTitle(): string {
    return this.isEditMode() ? 'Ürün Düzenle' : 'Yeni Ürün';
  }
}
