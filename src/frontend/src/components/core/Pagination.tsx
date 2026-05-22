import { useMemo } from "react";

import { ChevronLeft, ChevronRight } from "../Icons/PaginationIcons";
import { PageButton } from "./PageButton";

import { getPages } from "../../utils";

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
  const pages = useMemo(() => getPages(page, totalPages), [page, totalPages]);

  if (totalPages <= 1) return null;

  return (
    <div className="pagination">
      <span className="pagination__info">
        {totalItems ? `Showing ${start}–${end} of ${totalItems}` : `Page ${page} of ${totalPages}`}
      </span>

      <div className="pagination__buttons">
        <button className="page-btn" onClick={() => onPageChange(page - 1)} disabled={page === 1} aria-label="Previous page" >
          <ChevronLeft />
        </button>

        {pages.map((p, i) => (
          <PageButton key={`page-${i}`} p={p} page={page} onPageChange={onPageChange} index={i} />
        ))}

        <button className="page-btn" onClick={() => onPageChange(page + 1)} disabled={page === totalPages} aria-label="Next page" >
          <ChevronRight />
        </button>
      </div>
    </div>
  );
}