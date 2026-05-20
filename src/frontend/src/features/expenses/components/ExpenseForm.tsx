import { useState as useFormState } from "react";
 
export interface ExpenseFormData {
  description: string;
  note: string;
  category: string;
  date: string;
  amount: string;
}
 
interface ExpenseFormProps {
  initialData?: Partial<ExpenseFormData>;
  onSubmit: (data: ExpenseFormData) => void;
  onCancel: () => void;
}
 
const CATEGORIES = [
  "Food",
  "Travel",
  "Subscriptions",
  "Utilities",
  "Health",
  "Other",
];
 
export function ExpenseForm({ initialData, onSubmit, onCancel }: ExpenseFormProps) {
  const today = new Date().toISOString().split("T")[0];
 
  const [form, setForm] = useFormState<ExpenseFormData>({
    description: initialData?.description ?? "",
    note:        initialData?.note        ?? "",
    category:    initialData?.category    ?? "",
    date:        initialData?.date        ?? today,
    amount:      initialData?.amount      ?? "",
  });
 
  const [errors, setErrors] = useFormState<Partial<ExpenseFormData>>({});
 
  function validate(): boolean {
    const next: Partial<ExpenseFormData> = {};
    if (!form.description.trim()) next.description = "Required";
    if (!form.category)           next.category    = "Required";
    if (!form.date)               next.date        = "Required";
    if (!form.amount || isNaN(Number(form.amount)) || Number(form.amount) <= 0)
      next.amount = "Enter a valid amount";
    setErrors(next);
    return Object.keys(next).length === 0;
  }
 
  function handleSubmit() {
    if (validate()) onSubmit(form);
  }
 
  function field(key: keyof ExpenseFormData, value: string) {
    setForm((prev) => ({ ...prev, [key]: value }));
    setErrors((prev) => ({ ...prev, [key]: undefined }));
  }
 
  return (
    <>
      <div className="modal__header">
        <h2 className="modal__title">
          {initialData ? "Edit expense" : "New expense"}
        </h2>
        <button
          className="btn btn--ghost btn--icon"
          onClick={onCancel}
          aria-label="Close"
        >
          <svg width="16" height="16" viewBox="0 0 16 16" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" aria-hidden="true">
            <path d="M3 3l10 10M13 3L3 13" />
          </svg>
        </button>
      </div>
 
      <div>
        {/* Description */}
        <div className="form-group">
          <label className="form-label" htmlFor="exp-description">
            Description
          </label>
          <input
            id="exp-description"
            className="form-input"
            placeholder="e.g. Mercado do Bairro"
            value={form.description}
            onChange={(e) => field("description", e.target.value)}
          />
          {errors.description && (
            <p style={{ fontSize: 12, color: "var(--color-danger)", marginTop: 4 }}>
              {errors.description}
            </p>
          )}
        </div>
 
        {/* Note */}
        <div className="form-group">
          <label className="form-label" htmlFor="exp-note">
            Note <span style={{ fontWeight: 400, textTransform: "none", letterSpacing: 0 }}>(optional)</span>
          </label>
          <input
            id="exp-note"
            className="form-input"
            placeholder="e.g. Weekly groceries"
            value={form.note}
            onChange={(e) => field("note", e.target.value)}
          />
        </div>
 
        {/* Category */}
        <div className="form-group">
          <label className="form-label" htmlFor="exp-category">
            Category
          </label>
          <select
            id="exp-category"
            className="form-select"
            value={form.category}
            onChange={(e) => field("category", e.target.value)}
          >
            <option value="" disabled>Select a category</option>
            {CATEGORIES.map((c) => (
              <option key={c} value={c}>{c}</option>
            ))}
          </select>
          {errors.category && (
            <p style={{ fontSize: 12, color: "var(--color-danger)", marginTop: 4 }}>
              {errors.category}
            </p>
          )}
        </div>
 
        {/* Date + Amount */}
        <div className="form-row">
          <div className="form-group">
            <label className="form-label" htmlFor="exp-date">Date</label>
            <input
              id="exp-date"
              type="date"
              className="form-input"
              value={form.date}
              onChange={(e) => field("date", e.target.value)}
            />
            {errors.date && (
              <p style={{ fontSize: 12, color: "var(--color-danger)", marginTop: 4 }}>
                {errors.date}
              </p>
            )}
          </div>
 
          <div className="form-group">
            <label className="form-label" htmlFor="exp-amount">Amount (€)</label>
            <input
              id="exp-amount"
              type="number"
              min="0"
              step="0.01"
              className="form-input"
              placeholder="0.00"
              value={form.amount}
              onChange={(e) => field("amount", e.target.value)}
            />
            {errors.amount && (
              <p style={{ fontSize: 12, color: "var(--color-danger)", marginTop: 4 }}>
                {errors.amount}
              </p>
            )}
          </div>
        </div>
 
        <div className="form-footer">
          <button className="btn" onClick={onCancel}>Cancel</button>
          <button className="btn btn--primary" onClick={handleSubmit}>
            {initialData ? "Save changes" : "Add expense"}
          </button>
        </div>
      </div>
    </>
  );
}