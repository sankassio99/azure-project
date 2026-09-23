import { Expense } from '../models/expense.model';

export const MOCK_EXPENSES: Expense[] = [
  { id: 1, date: '2026-09-01', value: 42.5, description: 'Groceries', category: 'Food' },
  { id: 2, date: '2026-09-05', value: 15.0, description: 'Bus pass', category: 'Transport' },
  { id: 3, date: '2026-09-10', value: 89.99, description: 'Electricity bill', category: 'Utilities' },
  { id: 4, date: '2026-09-14', value: 12.75, description: 'Coffee with team', category: 'Food' },
  { id: 5, date: '2026-09-18', value: 250.0, description: 'Laptop stand', category: 'Equipment' },
];
