import { useCallback, useEffect, useState } from "react";
import type { Expense, PaginatedExpensesResponse } from "./types";
import { getExpenses, createExpense, updateExpense, deleteExpense } from "./api";
import type { CreateExpenseDto, UpdateExpenseDto } from "./types";

interface UseExpensesReturn {
  expenses: Expense[];
  pagination: PaginatedExpensesResponse | undefined;
  loading: boolean;
  error: string | null;
  add: (dto: CreateExpenseDto) => Promise<void>;
  edit: (id: string, dto: UpdateExpenseDto) => Promise<void>;
  remove: (id: string) => Promise<void>;
  refetch: () => void;
}

export function useExpenses(page = 1): UseExpensesReturn {
  const [expenses, setExpenses]   = useState<Expense[]>([]);
  const [pagination, setPagination] = useState<PaginatedExpensesResponse>();
  const [loading, setLoading]     = useState(true);
  const [error, setError]         = useState<string | null>(null);
  const [tick, setTick]           = useState(0);

  // Refetch trigger — call refetch() to re-run the effect
  const refetch = useCallback(() => setTick((t) => t + 1), []);

  useEffect(() => {
    let cancelled = false;

    const load = async () => {
      try {
        setLoading(true);
        setError(null);
        const response = await getExpenses(page);
        if (!cancelled) {
          setExpenses(response.items);
          setPagination(response);
        }
      } catch (err) {
        console.error(err);
        if (!cancelled) setError("Failed to load expenses");
      } finally {
        if (!cancelled) setLoading(false);
      }
    };

    load();
    return () => { cancelled = true; };
  }, [page, tick]);

  // ── Optimistic mutations ───────────────────────────────────

  const add = useCallback(async (dto: CreateExpenseDto) => {
    // Optimistic: prepend a temp item immediately
    const tempId = `temp-${crypto.randomUUID()}`;
    const optimistic: Expense = { id: tempId, ...dto };
    setExpenses((prev) => [optimistic, ...prev]);

    try {
      const created = await createExpense(dto);
      // Replace temp with real server response
      setExpenses((prev) => prev.map((e) => (e.id === tempId ? created : e)));
    } catch (err) {
      console.error(err);
      // Roll back
      setExpenses((prev) => prev.filter((e) => e.id !== tempId));
      throw err;
    }
  }, []);

  const edit = useCallback(async (id: string, dto: UpdateExpenseDto) => {
    // Optimistic: update locally first
    setExpenses((prev) =>
      prev.map((e) => (e.id === id ? { ...e, ...dto } : e))
    );

    try {
      const updated = await updateExpense(id, dto);
      // Reconcile with server response
      setExpenses((prev) => prev.map((e) => (e.id === id ? updated : e)));
    } catch (err) {
      console.error(err);
      // Roll back by refetching
      refetch();
      throw err;
    }
  }, [refetch]);

  const remove = useCallback(async (id: string) => {
    // Optimistic: remove immediately
    setExpenses((prev) => prev.filter((e) => e.id !== id));

    try {
      await deleteExpense(id);
    } catch (err) {
      console.error(err);
      // Roll back by refetching
      refetch();
      throw err;
    }
  }, [refetch]);

  return { expenses, pagination, loading, error, add, edit, remove, refetch };
}