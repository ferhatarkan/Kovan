/**
 * Backend'deki Kovan.Application.Features.Customers modellerinin karşılığıdır.
 */

/** Backend'deki Kovan.Domain.Enums.CustomerType karşılığıdır. */
export enum CustomerType {
  Individual = 1,
  Corporate = 2,
}

export interface Customer {
  id: string;
  displayName: string;
  customerType: CustomerType;
  email: string | null;
  phoneNumber: string;
  firstName?: string | null;
  lastName?: string | null;
  title?: string | null;
  taxOffice?: string;
  taxNumber?: string;
  nationalIdentityNumber?: string;
  address?: string;
}

export interface CreateCustomerRequest {
  customerType: CustomerType;
  firstName?: string | null;
  lastName?: string | null;
  nationalIdentityNumber?: string | null;
  title?: string | null;
  taxOffice?: string | null;
  taxNumber?: string | null;
  address: string;
  phoneNumber: string;
  email?: string | null;
}

export interface UpdateCustomerRequest extends CreateCustomerRequest {
  // Update için aynı yapı
}