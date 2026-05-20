import { useState } from "react";
import { TopBar } from "../components/TopBar";
import { Modal } from "../components/Modal";
import { ExpenseTable } from "../features/expenses/components/ExpenseTable";
import { ExpenseForm } from "../features/expenses/components/ExpenseForm";
import { Pagination } from "../components/Pagination";
import { useExpenses } from "../features/expenses/hooks";
import { useTheme } from "../hooks/useTheme";

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

function SectionToolbar({ count, onAdd, }: {
  count: number;
  onAdd: () => void;
}) {
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
// Derived summary metrics (extracted for clarity)
// ─────────────────────────────────────────────────────────────

function useSummaryMetrics(expenses: { amount: number }[]) {
  const totalSpent = expenses.reduce((sum, e) => sum + e.amount, 0);
  const largest = expenses.length
    ? Math.max(...expenses.map((e) => e.amount))
    : 0;

  const now = new Date();
  const monthLabel = now.toLocaleDateString("en-GB", {
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
  const { expenses, pagination, loading, error } = useExpenses();

  const [open, setOpen] = useState(false);
  const [page, setPage] = useState(1);

  const { totalSpent, largest, monthLabel } = useSummaryMetrics(expenses);

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

          <MetricCard label="Transactions" value={String(totalItems)} sub="This month" />
          <MetricCard label="Largest" value={`€${largest.toFixed(2)}`} sub="Single expense" />
        </div>

        {/* Toolbar */}
        <SectionToolbar count={totalItems} onAdd={() => setOpen(true)} />

        {/* Modal */}
        <Modal open={open} onClose={() => setOpen(false)}>
          <ExpenseForm
            onSubmit={(data) => {
              console.log(data);
              setOpen(false);
            }}
            onCancel={() => setOpen(false)}
          />
        </Modal>

        {/* States */}
        {loading && (<div className="state"> <div className="spinner" aria-label="Loading" /><p>Loading expenses…</p></div>)}

        {error && !loading && (<p className="error-text" role="alert">{error}</p>)}

        {/* Table + Pagination */}
        {!loading && !error && (
          <><ExpenseTable expenses={expenses.map((e) => ({ ...e, category: e.category || "", date: e.date || new Date().toISOString() }))} />

            <Pagination page={page} totalPages={totalPages} totalItems={totalItems} pageSize={pageSize} onPageChange={setPage} /></>
        )}
      </div>
    </div>
  );
}