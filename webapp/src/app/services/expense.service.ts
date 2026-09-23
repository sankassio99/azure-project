import { HttpClient } from '@angular/common/http';
import { Injectable, inject, signal } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { Expense, ExpenseInput } from '../models/expense.model';

const API_URL = '/api/expenses';

@Injectable({ providedIn: 'root' })
export class ExpenseService {
  private readonly http = inject(HttpClient);

  private readonly expenses = signal<Expense[]>([]);
  readonly expenses$ = this.expenses.asReadonly();

  async list(): Promise<Expense[]> {
    const data = await firstValueFrom(this.http.get<Expense[]>(API_URL));
    this.expenses.set(data);
    return data;
  }

  async getById(id: number): Promise<Expense> {
    return firstValueFrom(this.http.get<Expense>(`${API_URL}/${id}`));
  }

  async add(expense: ExpenseInput): Promise<Expense> {
    const created = await firstValueFrom(this.http.post<Expense>(API_URL, expense));
    this.expenses.update((current) => [...current, created]);
    return created;
  }

  async update(id: number, expense: ExpenseInput): Promise<Expense> {
    const updated = await firstValueFrom(this.http.put<Expense>(`${API_URL}/${id}`, expense));
    this.expenses.update((current) => current.map((item) => (item.id === id ? updated : item)));
    return updated;
  }

  async remove(id: number): Promise<void> {
    await firstValueFrom(this.http.delete<void>(`${API_URL}/${id}`));
    this.expenses.update((current) => current.filter((item) => item.id !== id));
  }
}
