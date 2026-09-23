import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ExpenseService } from '../../services/expense.service';

const CATEGORIES = ['Food', 'Transport', 'Utilities', 'Equipment', 'Entertainment', 'Other'];

@Component({
  selector: 'app-expense-form',
  imports: [ReactiveFormsModule],
  templateUrl: './expense-form.html',
  styleUrl: './expense-form.css',
})
export class ExpenseForm implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly expenseService = inject(ExpenseService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);

  protected readonly categories = CATEGORIES;
  protected readonly editingId = signal<number | null>(null);
  protected readonly isEditMode = computed(() => this.editingId() !== null);
  protected readonly loading = signal(false);
  protected readonly notFound = signal(false);
  protected readonly submitting = signal(false);

  protected readonly form = this.fb.nonNullable.group({
    date: ['', Validators.required],
    value: [0, [Validators.required, Validators.min(0.01)]],
    description: ['', Validators.required],
    category: ['', Validators.required],
  });

  async ngOnInit(): Promise<void> {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam === null) {
      return;
    }

    const id = Number(idParam);
    this.editingId.set(id);
    this.loading.set(true);
    try {
      const expense = await this.expenseService.getById(id);
      this.form.patchValue(expense);
    } catch {
      this.notFound.set(true);
    } finally {
      this.loading.set(false);
    }
  }

  async submit(): Promise<void> {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.submitting.set(true);
    const value = this.form.getRawValue();
    try {
      const id = this.editingId();
      if (id !== null) {
        await this.expenseService.update(id, value);
      } else {
        await this.expenseService.add(value);
      }
      this.router.navigate(['/expenses']);
    } finally {
      this.submitting.set(false);
    }
  }

  cancel(): void {
    this.router.navigate(['/expenses']);
  }
}
