// ─────────────────────────────────────────────────────────────
// Core entity
// ─────────────────────────────────────────────────────────────
 
export interface Expense {
  id: string;
  category: string;
  amount: number;
  createdAt: string;
}
 
// ─────────────────────────────────────────────────────────────
// API DTOs
// ─────────────────────────────────────────────────────────────
 
export type CreateExpenseDto = Omit<Expense, "id">;
 
export type UpdateExpenseDto = Partial<Omit<Expense, "id">>;
 
// ─────────────────────────────────────────────────────────────
// Paginated response
// ─────────────────────────────────────────────────────────────
 
export interface PaginatedExpensesResponse {
  items: Expense[];
  totalCount: number;
  totalPages: number;
  pageSize: number;
  currentPage: number;
}