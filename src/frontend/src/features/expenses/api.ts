import { http } from "../../api/http";
import type { Expense, PaginatedExpensesResponse, CreateExpenseDto, UpdateExpenseDto } from "./types";

export const getExpenses = async (page = 1, pageSize = 10): Promise<PaginatedExpensesResponse> => {
    const res = await http.get("/expenses", { params: { page, pageSize } });

    return res.data;
};

export const createExpense = async (dto: CreateExpenseDto): Promise<Expense> => {
    const res = await http.post("/expenses", dto);

    return res.data;
};

export const updateExpense = async (id: string, dto: UpdateExpenseDto): Promise<Expense> => {
    const res = await http.put(`/expenses/${id}`, dto);

    return res.data;
};

export const deleteExpense = async (id: string): Promise<void> => {

    await http.delete(`/expenses/${id}`);
};