import type { Expense } from "./types";
import type { ExpenseFormData } from "./components/ExpenseForm";
import { useExpenses } from "./hooks";

interface UseExpenseActionsProps {
    page: number;
    onAddSuccess: () => void;
    onEditSuccess: () => void;
}

export function useExpenseActions({ page, onAddSuccess, onEditSuccess }: UseExpenseActionsProps) {
    const { expenses, pagination, loading, error, add, edit, remove } = useExpenses(page);

    async function handleAdd(data: ExpenseFormData) {
        try {
            await add({
                category: data.category,
                amount: parseFloat(data.amount),
                createdAt: data.createdAt,
            });
            onAddSuccess();
        } catch {
            // TODO: toast
        }
    }

    async function handleEdit(editing: Expense, data: ExpenseFormData) {
        if (!editing) return;
        try {
            await edit(editing.id, {
                category: data.category,
                amount: parseFloat(data.amount),
                createdAt: data.createdAt,
            });
            onEditSuccess();
        } catch {
            // TODO: toast
        }
    }

    async function handleDelete(id: string) {
        if (!window.confirm("Delete this expense?")) return;
        try {
            await remove(id);
        } catch {
            // TODO: toast
        }
    }

    return { expenses, pagination, loading, error, handleAdd, handleEdit, handleDelete };
}