interface PageButtonProps {
  p: number | "…";
  page: number;
  onPageChange: (page: number) => void;
  index: number
}

export function PageButton({ p, page, onPageChange }: PageButtonProps) {
  if (p === "…") {
    return (
      <span className="page-btn page-btn--ellipsis">…</span>
    );
  }

  return (
    <button
      className={`page-btn ${p === page ? "page-btn--active" : ""}`}
      onClick={() => onPageChange(p)}
      aria-label={`Page ${p}`}
      aria-current={p === page ? "page" : undefined}
    >
      {p}
    </button>
  );
}