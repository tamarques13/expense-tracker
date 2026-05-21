import { useState } from "react";
import { TopBar } from "../components/TopBar";
import { Modal } from "../components/Modal";
import { ExpenseTable } from "../features/expenses/components/ExpenseTable";
import { ExpenseForm } from "../features/expenses/components/ExpenseForm";
import { Pagination } from "../components/Pagination";
import { useExpenses } from "../features/expenses/hooks";
import { useTheme } from "../hooks/useTheme";
import type { Expense } from "../features/expenses/types";
import type { ExpenseFormData } from "../features/expenses/components/ExpenseForm";

// ─────────────────────────────────────────────────────────────
// Metric Card
// ─────────────────────────────────────────────────────────────

interface MetricCardProps {
  label: string;
  value: string;
  sub?: React.ReactNode;
}

function MetricCard({ label, value, sub }: MetricCardProps) {
  return (
    <div className="metric-card">
      <p className="metric-card__label">{label}</p>
      <p className="metric-card__value">{value}</p>
      {sub && <div className="metric-card__sub">{sub}</div>}
    </div>
  );
}

// ─────────────────────────────────────────────────────────────
// Section Toolbar
// ─────────────────────────────────────────────────────────────

function SectionToolbar({ count, onAdd }: { count: number; onAdd: () => void }) {
  return (
    <div className="section-toolbar">
      <div className="section-toolbar__left">
        <span className="section-title">Recent</span>
        <span className="count-badge">{count} items</span>
      </div>

      <button className="btn btn--primary" onClick={onAdd}>
        <svg
          width="14"
          height="14"
          viewBox="0 0 14 14"
          fill="none"
          stroke="currentColor"
          strokeWidth="2"
          strokeLinecap="round"
          aria-hidden="true"
        >
          <path d="M7 1v12M1 7h12" />
        </svg>
        Add expense
      </button>
    </div>
  );
}

// ─────────────────────────────────────────────────────────────
// Summary metrics hook
// ─────────────────────────────────────────────────────────────

function useSummaryMetrics(expenses: { amount: number }[]) {
  const totalSpent = expenses.reduce((sum, e) => sum + e.amount, 0);
  const largest = expenses.length
    ? Math.max(...expenses.map((e) => e.amount))
    : 0;

  const monthLabel = new Date().toLocaleDateString("en-GB", {
    month: "long",
    year: "numeric",
  });

  return { totalSpent, largest, monthLabel };
}

// ─────────────────────────────────────────────────────────────
// Home Page
// ─────────────────────────────────────────────────────────────

export default function Home() {
  const { theme, toggle } = useTheme();

  const [page, setPage] = useState(1);
  const [addOpen, setAddOpen] = useState(false);
  const [editing, setEditing] = useState<Expense | null>(null);

  const { expenses, pagination, loading, error, add, edit, remove } = useExpenses(page);

  const { totalSpent, largest, monthLabel } = useSummaryMetrics(expenses);

  const totalItems = pagination?.totalCount ?? expenses.length;
  const totalPages = pagination?.totalPages ?? 1;
  const pageSize = pagination?.pageSize ?? 10;

  // ── Handlers ──────────────────────────────────────────────

  async function handleAdd(data: ExpenseFormData) {
    try {
      await add({ category: data.category, amount: parseFloat(data.amount), createdAt: data.createdAt });
      setAddOpen(false);
    } catch {
      // Show Toast
    }
  }

  async function handleEdit(data: ExpenseFormData) {
    if (!editing) return;
    try {
      await edit(editing.id, { category: data.category, amount: parseFloat(data.amount), createdAt: data.createdAt });
      setEditing(null);
    } catch {
      // Show Toast
    }
  }

  async function handleDelete(id: string) {
    if (!window.confirm("Delete this expense?")) return;
    try {
      await remove(id);
    } catch {
      // Show Toast
    }
  }

  function handleExpenseEdit(expense: Omit<Expense, "createdAt"> & { date: string }) {
    setEditing({ ...expense, createdAt: expense.date });
  }

  // ── Render ────────────────────────────────────────────────

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
            value={`€${largest.toFixed(2)}`}
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
              onSubmit={handleEdit}
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
              onEdit={handleExpenseEdit}
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