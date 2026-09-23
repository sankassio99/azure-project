import { HttpErrorResponse, HttpInterceptorFn, HttpResponse } from '@angular/common/http';
import { delay, Observable, of, throwError } from 'rxjs';
import { Expense } from '../models/expense.model';
import { MOCK_EXPENSES } from '../data/mock-expenses';

const API_PREFIX = '/api/expenses';
const SIMULATED_LATENCY_MS = 300;

// In-memory store shared across all requests for the lifetime of the app.
let store: Expense[] = MOCK_EXPENSES.map((expense) => ({ ...expense }));
let nextId = Math.max(0, ...store.map((expense) => expense.id)) + 1;

export const mockExpenseInterceptor: HttpInterceptorFn = (req, next) => {
  if (!req.url.startsWith(API_PREFIX)) {
    return next(req);
  }

  const idMatch = req.url.match(new RegExp(`^${API_PREFIX}/(\\d+)$`));
  const id = idMatch ? Number(idMatch[1]) : null;

  switch (req.method) {
    case 'GET': {
      if (id === null) {
        return respond(store);
      }
      const found = store.find((expense) => expense.id === id);
      return found ? respond(found) : notFound(id);
    }
    case 'POST': {
      const created: Expense = { ...(req.body as Omit<Expense, 'id'>), id: nextId++ };
      store = [...store, created];
      return respond(created, 201);
    }
    case 'PUT': {
      if (id === null || !store.some((expense) => expense.id === id)) {
        return notFound(id ?? -1);
      }
      const updated: Expense = { ...(req.body as Expense), id };
      store = store.map((expense) => (expense.id === id ? updated : expense));
      return respond(updated);
    }
    case 'DELETE': {
      if (id === null || !store.some((expense) => expense.id === id)) {
        return notFound(id ?? -1);
      }
      store = store.filter((expense) => expense.id !== id);
      return respond(null, 200);
    }
    default:
      return next(req);
  }

  function respond<T>(body: T, status = 200): Observable<HttpResponse<T>> {
    return of(new HttpResponse({ status, body })).pipe(delay(SIMULATED_LATENCY_MS));
  }

  function notFound(expenseId: number): Observable<never> {
    return throwError(
      () =>
        new HttpErrorResponse({
          status: 404,
          statusText: 'Not Found',
          url: req.url,
          error: { message: `Expense ${expenseId} not found` },
        }),
    ).pipe(delay(SIMULATED_LATENCY_MS));
  }
};
