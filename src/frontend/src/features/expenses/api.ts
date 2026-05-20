import { http } from "../../api/http";
import type { Expense, PaginatedExpensesResponse } from "./types";

export const getExpenses =
    async (): Promise<PaginatedExpensesResponse> => {
        const res = await http.get("/expenses");

        return res.data;
    };

export const postExpenses =
    async (): Promise<Expense> => {
        const res = await http.post("/expenses");

        return res.data;
    };