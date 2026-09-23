import { Component, inject, OnInit, signal } from '@angular/core';
import { DecimalPipe } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { Expense } from '../../models/expense.model';
import { ExpenseService } from '../../services/expense.service';

@Component({
  selector: 'app-expense-list',
  imports: [RouterLink, DecimalPipe],
  templateUrl: './expense-list.html',
  styleUrl: './expense-list.css',
})
export class ExpenseList implements OnInit {
  private readonly expenseService = inject(ExpenseService);
  private readonly router = inject(Router);

  protected readonly expenses = this.expenseService.expenses$;
  protected readonly loading = signal(true);
  protected readonly deletingId = signal<number | null>(null);

  async ngOnInit(): Promise<void> {
    this.loading.set(true);
    await this.expenseService.list();
    this.loading.set(false);
  }

  editExpense(expense: Expense): void {
    this.router.navigate(['/expenses', expense.id, 'edit']);
  }

  async deleteExpense(expense: Expense): Promise<void> {
    const confirmed = confirm(`Delete expense "${expense.description}"?`);
    if (!confirmed) {
      return;
    }
    this.deletingId.set(expense.id);
    try {
      await this.expenseService.remove(expense.id);
    } finally {
      this.deletingId.set(null);
    }
  }
}
