import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'expenses' },
  {
    path: 'expenses',
    loadComponent: () => import('./features/expense-list/expense-list').then((m) => m.ExpenseList),
  },
  {
    path: 'expenses/new',
    loadComponent: () => import('./features/expense-form/expense-form').then((m) => m.ExpenseForm),
  },
  {
    path: 'expenses/:id/edit',
    loadComponent: () => import('./features/expense-form/expense-form').then((m) => m.ExpenseForm),
  },
];
