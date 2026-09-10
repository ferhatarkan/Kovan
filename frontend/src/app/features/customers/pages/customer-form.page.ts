import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { Customer, CustomerType, CreateCustomerRequest, UpdateCustomerRequest } from '../../../core/domain/models/customer.model';
import { CustomerRepository } from '../../../core/domain/repositories/sales.repository';
import { ApiError } from '../../../core/infrastructure/api/api-error';

@Component({
  selector: 'app-customer-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './customer-form.page.html',
  styleUrl: './customer-form.page.scss',
})
export default class CustomerFormPage {
  private readonly fb = inject(FormBuilder);
  private readonly customerRepository = inject(CustomerRepository);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);

  readonly form: FormGroup;
  readonly customerId = signal<string | null>(null);
  readonly loading = signal(false);
  readonly errorMessage = signal<string | null>(null);
  readonly isEditMode = signal(false);
  readonly customerType = CustomerType;

  constructor() {
    this.form = this.fb.group({
      customerType: [CustomerType.Individual, Validators.required],
      firstName: [''],
      lastName: [''],
      title: [''],
      taxOffice: [''],
      taxNumber: [''],
      nationalIdentityNumber: [''],
      address: ['', Validators.required],
      phoneNumber: ['', Validators.required],
      email: ['', [Validators.email]],
    });

    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.customerId.set(id);
      this.isEditMode.set(true);
      this.loadCustomer(id);
    }

    this.form.get('customerType')?.valueChanges.subscribe((type) => {
      this.updateValidatorsBasedOnType(type);
    });
  }

  private updateValidatorsBasedOnType(type: CustomerType): void {
    const firstNameControl = this.form.get('firstName');
    const lastNameControl = this.form.get('lastName');
    const titleControl = this.form.get('title');
    const taxOfficeControl = this.form.get('taxOffice');
    const taxNumberControl = this.form.get('taxNumber');
    const nationalIdentityNumberControl = this.form.get('nationalIdentityNumber');

    if (type === CustomerType.Individual) {
      firstNameControl?.setValidators([Validators.required]);
      lastNameControl?.setValidators([Validators.required]);
      nationalIdentityNumberControl?.setValidators([Validators.required]);
      titleControl?.clearValidators();
      taxOfficeControl?.clearValidators();
      taxNumberControl?.clearValidators();
    } else {
      firstNameControl?.clearValidators();
      lastNameControl?.clearValidators();
      nationalIdentityNumberControl?.clearValidators();
      titleControl?.setValidators([Validators.required]);
      taxOfficeControl?.setValidators([Validators.required]);
      taxNumberControl?.setValidators([Validators.required]);
    }

    firstNameControl?.updateValueAndValidity();
    lastNameControl?.updateValueAndValidity();
    nationalIdentityNumberControl?.updateValueAndValidity();
    titleControl?.updateValueAndValidity();
    taxOfficeControl?.updateValueAndValidity();
    taxNumberControl?.updateValueAndValidity();
  }

  private loadCustomer(id: string): void {
    this.loading.set(true);
    this.customerRepository.getById(id).subscribe({
      next: (customer) => {
        this.form.patchValue({
          customerType: customer.customerType,
          firstName: customer.firstName || '',
          lastName: customer.lastName || '',
          title: customer.title || '',
          taxOffice: customer.taxOffice || '',
          taxNumber: customer.taxNumber || '',
          nationalIdentityNumber: customer.nationalIdentityNumber || '',
          address: customer.address || '',
          phoneNumber: customer.phoneNumber,
          email: customer.email || '',
        });
        this.loading.set(false);
      },
      error: (error: unknown) => {
        this.loading.set(false);
        this.errorMessage.set(
          error instanceof ApiError ? error.message : 'Müşteri bilgileri yüklenemedi.',
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

    if (this.isEditMode()) {
      const updateRequest: UpdateCustomerRequest = {
        ...formValue,
      };
      this.customerRepository.update(this.customerId()!, updateRequest).subscribe({
        next: () => {
          this.loading.set(false);
          void this.router.navigate(['../'], { relativeTo: this.route });
        },
        error: (error: unknown) => {
          this.loading.set(false);
          this.errorMessage.set(
            error instanceof ApiError ? error.message : 'Müşteri güncellenemedi.',
          );
        },
      });
    } else {
      const createRequest: CreateCustomerRequest = {
        ...formValue,
      };
      this.customerRepository.create(createRequest).subscribe({
        next: () => {
          this.loading.set(false);
          void this.router.navigate(['../'], { relativeTo: this.route });
        },
        error: (error: unknown) => {
          this.loading.set(false);
          this.errorMessage.set(
            error instanceof ApiError ? error.message : 'Müşteri oluşturulamadı.',
          );
        },
      });
    }
  }

  onCancel(): void {
    void this.router.navigate(['../'], { relativeTo: this.route });
  }

  get pageTitle(): string {
    return this.isEditMode() ? 'Müşteri Düzenle' : 'Yeni Müşteri';
  }
}