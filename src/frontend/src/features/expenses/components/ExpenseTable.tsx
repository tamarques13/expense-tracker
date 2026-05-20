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
  Food:          "cat-food",
  Travel:        "cat-travel",
  Subscriptions: "cat-sub",
  Utilities:     "cat-util",
  Health:        "cat-health",
};
 
function formatDate(iso: string): string {
  const d = new Date(iso);
  return d.toLocaleDateString("en-GB", { day: "numeric", month: "short" });
}
 
export function ExpenseTable({ expenses, onEdit, onDelete }: ExpenseTableProps) {
  if (expenses.length === 0) {
    return (
      <div className="table-wrapper">
        <div className="state">
          <p>No expenses yet.</p>
          <p style={{ fontSize: 13, marginTop: 4 }}>
            Click <strong>Add expense</strong> to get started.
          </p>
        </div>
      </div>
    );
  }
 
  return (
    <div className="table-wrapper">
      <table>
        <thead>
          <tr>
            <th className="col-description">Description</th>
            <th className="col-category">Category</th>
            <th className="col-date">Date</th>
            <th className="col-amount">Amount</th>
            <th className="col-actions" />
          </tr>
        </thead>
        <tbody>
          {expenses.map((expense) => {
            const catClass = CATEGORY_CLASS[expense.category] ?? "cat-other";
            return (
              <tr key={expense.id}>
                <td>
                  <div className="row-main">{expense.category}</div>
                </td>
 
                <td>
                  <span className={`cat-pill ${catClass}`}>
                    <span className="cat-dot" aria-hidden="true" />
                    {expense.category}
                  </span>
                </td>
 
                <td>
                  <span className="row-date">{formatDate(expense.date)}</span>
                </td>
 
                <td>
                  <span className="row-amount row-amount--negative">
                    −€{expense.amount.toFixed(2)}
                  </span>
                </td>
 
                <td>
                  <div className="row-actions">
                    {onEdit && (
                      <button
                        className="action-btn"
                        onClick={() => onEdit(expense)}
                        aria-label={`Edit ${expense.category}`}
                      >
                        {/* Pencil icon */}
                        <svg width="14" height="14" viewBox="0 0 14 14" fill="none" stroke="currentColor" strokeWidth="1.7" strokeLinecap="round" strokeLinejoin="round" aria-hidden="true">
                          <path d="M9.5 2.5 11.5 4.5 4.5 11.5l-2.5.5.5-2.5z" />
                        </svg>
                      </button>
                    )}
                    {onDelete && (
                      <button
                        className="action-btn action-btn--danger"
                        onClick={() => onDelete(expense.id)}
                        aria-label={`Delete ${expense.category}`}
                      >
                        {/* Trash icon */}
                        <svg width="14" height="14" viewBox="0 0 14 14" fill="none" stroke="currentColor" strokeWidth="1.7" strokeLinecap="round" strokeLinejoin="round" aria-hidden="true">
                          <path d="M2 3.5h10M5.5 3.5V2h3v1.5M5.5 6v4M8.5 6v4M3 3.5l.7 8h6.6l.7-8" />
                        </svg>
                      </button>
                    )}
                  </div>
                </td>
              </tr>
            );
          })}
        </tbody>
      </table>
    </div>
  );
}