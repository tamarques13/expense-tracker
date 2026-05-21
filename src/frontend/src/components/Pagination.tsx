interface PaginationProps {
  page: number;
  totalPages: number;
  totalItems?: number;
  pageSize?: number;
  onPageChange: (page: number) => void;
}

export function Pagination({ page, totalPages, totalItems, pageSize = 10, onPageChange, }: PaginationProps) {

  const start = (page - 1) * pageSize + 1;
  const end = Math.min(page * pageSize, totalItems ?? page * pageSize);

  function getPages(): (number | "…")[] {
    if (totalPages <= 5) {
      return Array.from({ length: totalPages }, (_, i) => i + 1);
    }
    if (page <= 3) return [1, 2, 3, 4, "…", totalPages];
    if (page >= totalPages - 2) return [1, "…", totalPages - 3, totalPages - 2, totalPages - 1, totalPages];
    return [1, "…", page - 1, page, page + 1, "…", totalPages];
  }

  return (
    <div className="pagination">
      <span className="pagination__info">
        {totalItems ? `Showing ${start}–${end} of ${totalItems}` : `Page ${page} of ${totalPages}`}
      </span>

      <div className="pagination__buttons">
        <button className="page-btn" onClick={() => onPageChange(page - 1)} disabled={page === 1} aria-label="Previous page" >

          {/* Chevron left */}
          <svg width="12" height="12" viewBox="0 0 12 12" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round" aria-hidden="true">
            <path d="M8 2L4 6l4 4" />
          </svg>
        </button>

        {getPages().map((p, i) => p === "…" ? (
          <span key={`ellipsis-${i}`} className="page-btn" style={{ cursor: "default", color: "var(--color-text-muted)" }} > … </span>) : (
          <button
            key={p}
            className={`page-btn ${p === page ? "page-btn--active" : ""}`}
            onClick={() => onPageChange(p as number)}
            aria-label={`Page ${p}`}
            aria-current={p === page ? "page" : undefined}
          >
            {p}
          </button>))}

        <button
          className="page-btn"
          onClick={() => onPageChange(page + 1)}
          disabled={page === totalPages}
          aria-label="Next page"
        >

          {/* Chevron right */}
          <svg width="12" height="12" viewBox="0 0 12 12" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round" aria-hidden="true">
            <path d="M4 2l4 4-4 4" />
          </svg>
        </button>
      </div>
    </div>
  );
}