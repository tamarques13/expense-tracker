export interface Expense {
  id: string;
  category: string;
  date: string;       // ISO date string e.g. "2026-04-28"
  amount: number;     // always positive; displayed as negative outflow
}

interface ExpenseTableProps {
  expenses: Expense[];
  onEdit?: (expense: Expense) => void;
  onDelete?: (id: string) => void;
}

const CATEGORY_CLASS: Record<string, string> = {
  Food: "cat-food",
  Groceries: "cat-groceries",
  Transport: "cat-transport",
  Housing: "cat-housing",
  Utilities: "cat-utilities",
  Health: "cat-health",
  Entertainment: "cat-entertainment",
  Shopping: "cat-shopping",
  Other: "cat-other",
};

function formatAmount(amount: number) {
  return new Intl.NumberFormat("pt-PT", {
    style: "currency",
    currency: "EUR",
  }).format(-amount);
}

function formatDate(iso: string): string {
  const d = new Date(iso);
  return d.toLocaleDateString("en-GB", {
    day: "numeric",
    month: "short",
  });
}

function RowActions({ expense, onEdit, onDelete }: {
  expense: Expense;
  onEdit?: (expense: Expense) => void;
  onDelete?: (id: string) => void;
}) {
  return (
    <div className="row-actions">
      {onEdit && (
        <button className="action-btn" onClick={() => onEdit(expense)}>
          <svg width="14" height="14" viewBox="0 0 14 14">
            <path d="M9.5 2.5 11.5 4.5 4.5 11.5l-2.5.5.5-2.5z" />
          </svg>
        </button>
      )}
      {onDelete && (
        <button
          className="action-btn action-btn--danger"
          onClick={() => onDelete(expense.id)}
        >
          <svg width="14" height="14" viewBox="0 0 14 14">
            <path d="M2 3.5h10M5.5 3.5V2h3v1.5M5.5 6v4M8.5 6v4M3 3.5l.7 8h6.6l.7-8" />
          </svg>
        </button>
      )}
    </div>
  );
}

export function ExpenseTable({ expenses, onEdit, onDelete }: ExpenseTableProps) {
  if (expenses.length === 0) {
    return (
      <div className="table-wrapper">
        <div className="state">
          <p>No expenses yet.</p>
          <p className="state-sub">Click <strong>Add expense</strong> to get started.</p>
        </div>
      </div>
    );
  }

  return (
    <div className="table-wrapper">
      <table>
        <thead>
          <tr>
            <th>Description</th>
            <th>Category</th>
            <th>Date</th>
            <th>Amount</th>
            <th></th>
          </tr>
        </thead>

        <tbody>
          {expenses.map((expense) => {
            const catClass = CATEGORY_CLASS[expense.category] ?? "cat-other";

            return (
              <tr key={expense.id}>
                <td>{expense.category}</td>

                <td>
                  <span className={`cat-pill ${catClass}`}>
                    <span className="cat-dot" />
                    {expense.category}
                  </span>
                </td>

                <td>{formatDate(expense.date)}</td>

                <td className="row-amount row-amount--negative">
                  {formatAmount(expense.amount)}
                </td>

                <td>
                  <RowActions
                    expense={expense}
                    onEdit={onEdit}
                    onDelete={onDelete}
                  />
                </td>
              </tr>
            );
          })}
        </tbody>
      </table>
    </div>
  );
}