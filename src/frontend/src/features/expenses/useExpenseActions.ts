import type { Expense } from "./types";
import type { ExpenseFormData } from "./components/ExpenseForm";
import { useExpenses } from "./hooks";

interface UseExpenseActionsProps {
    page: number;
    onAddSuccess: () => void;
    onEditSuccess: () => void;
    showToast: (message: string, type?: "success" | "error") => void;
}

export function useExpenseActions({ page, onAddSuccess, onEditSuccess, showToast }: UseExpenseActionsProps) {
    const { expenses, pagination, loading, error, add, edit, remove } = useExpenses(page);

    async function handleAdd(data: ExpenseFormData) {
        try {
            await add({
                category: data.category,
                amount: parseFloat(data.amount),
                createdAt: data.createdAt,
            });
            onAddSuccess();
            showToast("Expense Created");
        } catch {
            showToast("Failed to add expense", "error");
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
            showToast("Expense updated");
        } catch {
            showToast("Failed to update expense", "error");
        }
    }

    async function handleDelete(id: string) {
        if (!window.confirm("Delete this expense?")) return;
        try {
            await remove(id);
            showToast("Expense Deleted");
        } catch {
            showToast("Failed to delete expense", "error");
        }
    }

    return { expenses, pagination, loading, error, handleAdd, handleEdit, handleDelete };
}