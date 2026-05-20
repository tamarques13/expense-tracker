import { useEffect, useState } from "react";
import type { Expense, PaginatedExpensesResponse } from "./types";

import { getExpenses } from "./api";

export function useExpenses() {
    const [expenses, setExpenses] = useState<Expense[]>([]);
    const [pagination, setPagination] = useState<PaginatedExpensesResponse>();
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const load = async () => {
            try {
                setLoading(true);

                const response: PaginatedExpensesResponse = await getExpenses();

                setExpenses(response.items);
                setPagination(response)
            } catch (err) {
                console.error(err);
                setError("Failed to load expenses");
            } finally {
                setLoading(false);
            }
        };

        load();
    }, []);

    return {
        expenses,
        pagination,
        loading,
        error,
    };
}