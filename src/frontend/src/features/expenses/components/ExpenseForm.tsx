import { useState as useFormState } from "react";

export interface ExpenseFormData {
  category: string;
  createdAt: string;
  amount: string;
}

interface ExpenseFormProps {
  initialData?: Partial<ExpenseFormData>;
  onSubmit: (data: ExpenseFormData) => void;
  onCancel: () => void;
}

const CATEGORIES = [
  "Food",
  "Groceries",
  "Transport",
  "Housing",
  "Utilities",
  "Health",
  "Entertainment",
  "Shopping",
  "Other",
] as const;

export function ExpenseForm({ initialData, onSubmit, onCancel }: ExpenseFormProps) {
  const today = new Date().toISOString().split("T")[0];

  const [form, setForm] = useFormState<ExpenseFormData>({
    category: initialData?.category ?? "",
    createdAt: initialData?.createdAt ?? today,
    amount: initialData?.amount ?? "",
  });

  const [errors, setErrors] = useFormState<Partial<ExpenseFormData>>({});

  const update = (key: keyof ExpenseFormData, value: string) => {
    setForm((prev) => ({ ...prev, [key]: value }));
    setErrors((prev) => ({ ...prev, [key]: undefined }));
  };

  const validate = () => {
    const next: Partial<ExpenseFormData> = {};

    if (!form.category) next.category = "Required";
    if (!form.createdAt) next.createdAt = "Required";

    const amount = Number(form.amount);
    if (!amount || amount <= 0) next.amount = "Enter a valid amount";

    setErrors(next);
    return Object.keys(next).length === 0;
  };

  const handleSubmit = () => { if (validate()) onSubmit(form); };

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
          <svg width="16" height="16" viewBox="0 0 16 16" stroke="currentColor" strokeWidth="2">
            <path d="M3 3l10 10M13 3L3 13" />
          </svg>
        </button>
      </div>

      <div>
        {/* Category */}
        <FormGroup label="Category" error={errors.category}>
          <select
            id="exp-category"
            className="form-select"
            value={form.category}
            onChange={(e) => update("category", e.target.value)}
          >
            <option value="" disabled>Select a category</option>
            {CATEGORIES.map((c) => (<option className="form-option" key={c} value={c}>{c}</option>))}
          </select>
        </FormGroup>

        {/* Amount */}
        <div>
          <FormGroup label="Amount (€)" error={errors.amount}>
            <input
              id="exp-amount"
              type="number"
              min="0"
              step="0.01"
              className="form-input"
              placeholder="0.00"
              value={form.amount}
              onChange={(e) => update("amount", e.target.value)}
            />
          </FormGroup>
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

/* Extracted reusable component */
function FormGroup({ label, error, children }: {
  label: string;
  error?: string;
  children: React.ReactNode;
}) {
  return (
    <div className="form-group">
      <label className="form-label">{label}</label>
      {children}
      {error && (<p className="form-error">{error}</p>)}
    </div>
  );
}
