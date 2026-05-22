import { useState, useMemo } from "react";

import { TopBar } from "../components/core/TopBar";
import { Modal } from "../components/core/Modal";
import { Pagination } from "../components/core/Pagination";
import { Toast } from "../components/core/Toast";
import { SectionToolbar } from "../components/home/SectionToolbar";
import { Header } from "../components/home/PageHeader";

import { MetricCard } from "../features/analytics/components/MetricCard";
import { ExpenseTable } from "../features/expenses/components/ExpenseTable";
import { ExpenseForm } from "../features/expenses/components/ExpenseForm";

import { useAnalytics } from "../features/analytics/hooks";
import { useTheme } from "../hooks/useTheme";
import { useExpenseActions } from "../features/expenses/useExpenseActions";
import { useToast } from "../hooks/useToast";

import type { Expense } from "../features/expenses/types";

/**
 * Home dashboard page.
 * Shows monthly analytics and expense metrics.
 * Displays expenses table with pagination.
 * Supports adding and editing expenses via modals.
 * Uses theme, analytics and expense action hooks.
 */

const currentDate = new Date();
const today = currentDate.toISOString().split("T")[0];
const monthLabel = currentDate.toLocaleDateString("en-GB", { month: "long", year: "numeric" });

export default function Home() {

  const { theme, toggle } = useTheme();
  const [page, setPage] = useState(1);
  const [addOpen, setAddOpen] = useState(false);
  const [editing, setEditing] = useState<Expense | null>(null);

  const { toast, showToast, dismiss } = useToast();
  const { analytics } = useAnalytics(today);
  const { expenses, pagination, loading, error, handleAdd, handleEdit, handleDelete } = useExpenseActions({
    page,
    onAddSuccess: () => setAddOpen(false),
    onEditSuccess: () => setEditing(null),
    showToast,
  });

  const totalMonthSpend = analytics?.amounts.total ?? 0;
  const bgMonthSpend = analytics?.amounts.largest ?? 0;
  const totalItems = pagination?.totalCount ?? expenses.length;
  const totalPages = pagination?.totalPages ?? 1;
  const pageSize = pagination?.pageSize ?? 10;
  const tableExpenses = useMemo(() => expenses.map((e) => ({ ...e, date: e.createdAt })), [expenses]);

  return (
    <div className="app-shell" data-theme={theme}>
      <TopBar theme={theme} toggle={toggle} />

      <div className="container">
        <Header monthLabel={monthLabel} />

        <div className="metrics-grid">
          <MetricCard
            label="Total spent"
            value={`€${totalMonthSpend.toLocaleString("en-IE", { minimumFractionDigits: 2, maximumFractionDigits: 2, })}`}
            sub="This month"
          />
          <MetricCard
            label="Transactions"
            value={String(totalItems)}
            sub="This month"
          />
          <MetricCard
            label="Largest"
            value={`€${bgMonthSpend.toFixed(2)}`}
            sub="Single expense"
          />
        </div>

        <SectionToolbar count={totalItems} onAdd={() => setAddOpen(true)} />

        <Modal open={addOpen} onClose={() => setAddOpen(false)}>
          <ExpenseForm onSubmit={handleAdd} onCancel={() => setAddOpen(false)} />
        </Modal>

        <Modal open={!!editing} onClose={() => setEditing(null)}>
          {editing && (
            <ExpenseForm
              initialData={{
                category: editing.category,
                createdAt: editing.createdAt,
                amount: String(editing.amount),
              }}
              onSubmit={(values) => handleEdit(editing, values)}
              onCancel={() => setEditing(null)}
            />
          )}
        </Modal>

        {loading && (
          <div className="state">
            <div className="spinner" aria-label="Loading" />
            <p>Loading expenses…</p>
          </div>
        )}

        {error && !loading && (<p className="error-text" role="alert">{error}</p>)}

        {!loading && !error && (
          <>
            <ExpenseTable
              expenses={tableExpenses}
              onEdit={(expense) => setEditing(expense)}
              onDelete={handleDelete}
            />

            <Pagination page={page} totalPages={totalPages} totalItems={totalItems} pageSize={pageSize} onPageChange={setPage} />
          </>
        )}

        {/* Toast */}
        {toast && (<Toast key={toast.id} message={toast.message} type={toast.type} onDone={dismiss} />)}

      </div>
    </div>
  );
}