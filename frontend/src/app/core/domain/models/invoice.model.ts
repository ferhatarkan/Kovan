/**
 * Backend'deki Kovan.Application.Features.Invoices modellerinin karşılığıdır.
 */

/** Backend'deki Kovan.Domain.Enums.InvoiceStatus karşılığıdır. */
export enum InvoiceStatus {
  Draft = 0,
  Sent = 1,
  PartiallyPaid = 2,
  Paid = 3,
  Overdue = 4,
}

/** Backend'deki Kovan.Domain.Enums.PaymentMethod karşılığıdır. */
export enum PaymentMethod {
  Cash = 1,
  CreditCard = 2,
  BankTransfer = 3,
  Other = 99,
}

export interface Invoice {
  id: string;
  invoiceNumber: string;
  customerName: string;
  dueDate: string;
  grandTotal: number;
  status: InvoiceStatus;
}

export interface AddPaymentRequest {
  amount: number;
  paymentMethod: PaymentMethod;
  paymentDate?: string;
  notes?: string | null;
}

export interface CreateInvoiceRequest {
  customerId: string;
  warehouseId: string;
  invoiceNumber: string;
  dueDate: string;
  invoiceLines: CreateInvoiceLineDto[];
}

export interface CreateInvoiceLineDto {
  productId: string;
  quantity: number;
  unitPrice: number;
  vatRate: number;
}

export interface UpdateInvoiceRequest {
  id: string;
  invoiceNumber: string;
  dueDate: string;
  lines: UpdateInvoiceLineItem[];
}

export interface UpdateInvoiceLineItem {
  productId: string;
  quantity: number;
  unitPrice: number;
  vatRate: number;
}