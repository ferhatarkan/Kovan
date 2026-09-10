/**
 * Backend'deki Kovan.Application.Features.Suppliers modellerinin karşılığıdır.
 */

export interface Supplier {
  id: string;
  name: string;
  contactPerson: string;
  email: string;
  phone: string;
  address: string;
  city: string;
  country: string;
  taxNumber: string;
  isActive: boolean;
}

export interface CreateSupplierRequest {
  name: string;
  contactPerson: string;
  email: string;
  phone: string;
  address: string;
  city: string;
  country: string;
  taxNumber: string;
}

export type UpdateSupplierRequest = CreateSupplierRequest;
