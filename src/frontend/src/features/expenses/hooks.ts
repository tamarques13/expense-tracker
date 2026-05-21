import { useEffect, useState } from "react";
import { getExpenses, createExpense, updateExpense, deleteExpense } from "./api";
import type { Expense, CreateExpenseDto, UpdateExpenseDto, PaginatedExpensesResponse } from "./types";

export function useExpenses(page = 1) {
  const [expenses, setExpenses] = useState<Expense[]>([]);
  const [pagination, setPagination] = useState<PaginatedExpensesResponse>();
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  // Load expenses when page changes
  useEffect(() => {
    async function load() {
      try {
        setLoading(true);
        setError(null);

        const response = await getExpenses(page);

        setExpenses(response.items);
        setPagination(response);

      } catch (err) {
        console.error(err);
        setError("Failed to load expenses");

      } finally {
        setLoading(false);
      }
    }

    load();
  }, [page]);

  // Add new expense
  async function add(dto: CreateExpenseDto) {
    const tempId = `temp-${crypto.randomUUID()}`;
    const optimistic: Expense = { id: tempId, ...dto };
    setExpenses((prev) => [optimistic, ...prev]);

    try {
      setLoading(true);
      const created = await createExpense(dto);

      // Add the new item to the list
      setExpenses((prev) => prev.map((e) => (e.id === tempId ? created : e)));

    } catch (err) {
      console.error(err);

      setExpenses((prev) => prev.filter((e) => e.id !== tempId));
      setError("Failed to add expense");

    } finally {
      setLoading(false);
    }
  }

  // Edit an expense
  async function edit(id: string, dto: UpdateExpenseDto) {
    let previous: Expense | undefined;

    setExpenses((prev) => {
      previous = prev.find((e) => e.id === id);
      return prev.map((e) => (e.id === id ? { ...e, ...dto } : e));
    });


    try {
      setLoading(true);
      await updateExpense(id, dto);

      // Replace the old item with the updated one
      setExpenses((prev) => prev.map((e) => (e.id === id ? { ...e, ...dto } : e)));

    } catch (err) {
      console.error(err);

      if (previous) setExpenses((prev) => prev.map((e) => (e.id === id ? previous! : e)));
      setError("Failed to update expense");

    } finally {
      setLoading(false);
    }
  }

  // Delete an expense
  async function remove(id: string) {
    let removed: Expense | undefined;
    let removedIndex = -1;

    setExpenses((prev) => {
      removedIndex = prev.findIndex((e) => e.id === id);
      removed = prev[removedIndex];
      return prev.filter((e) => e.id !== id);
    });

    try {
      setLoading(true);
      await deleteExpense(id);

      // Remove from list
      setExpenses((prev) => prev.filter((e) => e.id !== id));

    } catch (err) {
      console.error(err);

      if (removed !== undefined && removedIndex !== -1) {
        setExpenses((prev) => {
          const next = [...prev];
          next.splice(removedIndex, 0, removed!);
          return next;
        });
      }
      setError("Failed to delete expense");

    } finally {
      setLoading(false);
    }
  }

  return {
    expenses,
    pagination,
    loading,
    error,
    add,
    edit,
    remove,
  };
}