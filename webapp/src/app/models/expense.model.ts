export interface Expense {
  id: number;
  date: string;
  value: number;
  description: string;
  category: string;
}

export type ExpenseInput = Omit<Expense, 'id'>;
