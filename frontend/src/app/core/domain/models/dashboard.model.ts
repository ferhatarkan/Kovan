/**
 * Backend'deki Kovan.Application.Features.Dashboard modellerinin karşılığıdır.
 */

export interface DashboardDto {
  totalInvoices: number;
  totalCustomers: number;
  totalProducts: number;
  totalRevenue: number;
  invoiceStatusSummary: InvoiceStatusSummary;
  topSellingProducts: TopSellingProductDto[];
}

export interface InvoiceStatusSummary {
  paid: number;
  partiallyPaid: number;
  draft: number;
}

export interface TopSellingProductDto {
  productName: string;
  totalQuantitySold: number;
}
