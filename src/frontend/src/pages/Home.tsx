import { useState } from "react";

import { TopBar } from "../components/TopBar";
import { Modal } from "../components/Modal";
import { Pagination } from "../components/Pagination";

import { SectionToolbar } from "../components/home/SectionToolbar";

import { MetricCard } from "../features/analytics/components/MetricCard";
import { ExpenseTable } from "../features/expenses/components/ExpenseTable";
import { ExpenseForm } from "../features/expenses/components/ExpenseForm";

import { useAnalytics } from "../features/analytics/hooks";
import { useTheme } from "../hooks/useTheme";
import { useExpenseActions } from "../features/expenses/useExpenseActions";

import type { Expense } from "../features/expenses/types";

/**
 * Home dashboard page.
 * Shows monthly analytics and expense metrics.
 * Displays expenses table with pagination.
 * Supports adding and editing expenses via modals.
 * Uses theme, analytics and expense action hooks.
 */

export default function Home() {
  const currentDate = new Date();

  const today = currentDate.toISOString().split("T")[0];
  const monthLabel = currentDate.toLocaleDateString("en-GB", { month: "long", year: "numeric" });


  const { theme, toggle } = useTheme();  
  const [page, setPage] = useState(1);
  const [date] = useState(today);
  const [addOpen, setAddOpen] = useState(false);
  const [editing, setEditing] = useState<Expense | null>(null);

  const { expenses, pagination, loading, error, handleAdd, handleEdit, handleDelete } = useExpenseActions({
    page,
    onAddSuccess: () => setAddOpen(false),
    onEditSuccess: () => setEditing(null),
  });

  const { analytics } = useAnalytics(date);


  // Analytics
  const totalSpent = analytics?.amounts.total ?? 0;
  const lgSpent = analytics?.amounts.largest ?? 0;

  // Pagination
  const totalItems = pagination?.totalCount ?? expenses.length;
  const totalPages = pagination?.totalPages ?? 1;
  const pageSize = pagination?.pageSize ?? 10;

  return (
    <div className="app-shell" data-theme={theme}>
      <TopBar theme={theme} toggle={toggle} />

      <div className="container">

        {/* Page header */}
        <div className="page-header">
          <p className="page-eyebrow">{monthLabel}</p>
          <h1 className="page-title">Expenses</h1>
        </div>

        {/* Metrics */}
        <div className="metrics-grid">
          <MetricCard
            label="Total spent"
            value={`€${totalSpent.toLocaleString("en-IE", {
              minimumFractionDigits: 2,
              maximumFractionDigits: 2,
            })}`}
            sub="This month"
          />
          <MetricCard
            label="Transactions"
            value={String(totalItems)}
            sub="This month"
          />
          <MetricCard
            label="Largest"
            value={`€${lgSpent.toFixed(2)}`}
            sub="Single expense"
          />
        </div>

        {/* Toolbar */}
        <SectionToolbar count={totalItems} onAdd={() => setAddOpen(true)} />

        {/* Add modal */}
        <Modal open={addOpen} onClose={() => setAddOpen(false)}>
          <ExpenseForm
            onSubmit={handleAdd}
            onCancel={() => setAddOpen(false)}
          />
        </Modal>

        {/* Edit modal */}
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

        {/* Loading */}
        {loading && (
          <div className="state">
            <div className="spinner" aria-label="Loading" />
            <p>Loading expenses…</p>
          </div>
        )}

        {/* Error */}
        {error && !loading && (
          <p className="error-text" role="alert">{error}</p>
        )}

        {/* Table + Pagination */}
        {!loading && !error && (
          <>
            <ExpenseTable
              expenses={expenses.map((expense) => ({
                ...expense,
                date: expense.createdAt,
              }))}
              onEdit={(expense) => setEditing(expense)}
              onDelete={handleDelete}
            />

            <Pagination
              page={page}
              totalPages={totalPages}
              totalItems={totalItems}
              pageSize={pageSize}
              onPageChange={setPage}
            />
          </>
        )}

      </div>
    </div>
  );
}