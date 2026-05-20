export type Expense = {
  date: string;
  id: string;
  category: string;
  amount: number;
  createdAt: string;
};

export type PaginatedExpensesResponse = {
  items: Expense[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
};