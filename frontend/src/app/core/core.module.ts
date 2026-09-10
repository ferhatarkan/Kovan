import { EnvironmentProviders, Provider } from '@angular/core';
import { provideHttpClient, withInterceptors } from '@angular/common/http';

import { AuthRepository } from './domain/repositories/auth.repository';
import { CategoryRepository, ProductRepository } from './domain/repositories/catalog.repository';
import { CustomerRepository, InvoiceRepository } from './domain/repositories/sales.repository';
import { DashboardRepository } from './domain/repositories/dashboard.repository';

import { AuthApiRepository } from './infrastructure/repositories/auth-api.repository';
import {
  CategoryApiRepository,
  ProductApiRepository,
} from './infrastructure/repositories/catalog-api.repository';
import {
  CustomerApiRepository,
  InvoiceApiRepository,
} from './infrastructure/repositories/sales-api.repository';
import { DashboardApiRepository } from './infrastructure/repositories/dashboard-api.repository';

import { authInterceptor } from './infrastructure/interceptors/auth.interceptor';

/**
 * Core katmanının Dependency Injection kayıtları.
 * Backend'deki DependencyInjection.cs yapısının frontend karşılığıdır:
 * domain port'ları infrastructure implementasyonlarına bağlanır.
 */
export function provideCore(): Array<Provider | EnvironmentProviders> {
  return [
    provideHttpClient(withInterceptors([authInterceptor])),
    { provide: AuthRepository, useClass: AuthApiRepository },
    { provide: ProductRepository, useClass: ProductApiRepository },
    { provide: CategoryRepository, useClass: CategoryApiRepository },
    { provide: CustomerRepository, useClass: CustomerApiRepository },
    { provide: InvoiceRepository, useClass: InvoiceApiRepository },
    { provide: DashboardRepository, useClass: DashboardApiRepository },
  ];
}