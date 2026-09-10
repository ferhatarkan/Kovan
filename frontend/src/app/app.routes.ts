import { Routes } from '@angular/router';

import { authGuard } from './presentation/guards/auth.guard';
import { LayoutComponent } from './presentation/layout/layout.component';

export const routes: Routes = [
  {
    path: 'auth',
    children: [
      {
        path: 'login',
        loadComponent: () => import('./features/auth/pages/login.page').then((m) => m.default),
      },
    ],
  },
  {
    path: '',
    component: LayoutComponent,
    canActivate: [authGuard],
    children: [
      { path: '', pathMatch: 'full', redirectTo: 'products' },
      {
        path: 'products',
        loadComponent: () =>
          import('./features/products/pages/product-list.page').then((m) => m.default),
      },
      {
        path: 'customers',
        children: [
          {
            path: '',
            loadComponent: () =>
              import('./features/customers/pages/customer-list.page').then((m) => m.default),
          },
          {
            path: 'new',
            loadComponent: () =>
              import('./features/customers/pages/customer-form.page').then((m) => m.default),
          },
          {
            path: ':id',
            loadComponent: () =>
              import('./features/customers/pages/customer-form.page').then((m) => m.default),
          },
        ],
      },
      {
        path: 'invoices',
        loadComponent: () =>
          import('./features/invoices/pages/invoice-list.page').then((m) => m.default),
      },
    ],
  },
  { path: '**', redirectTo: '' },
];