// utils/pagination.ts

export function getPages(page: number, totalPages: number): (number | "…")[] {
    if (totalPages <= 5) return Array.from({ length: totalPages }, (_, i) => i + 1);
    if (page <= 3) return [1, 2, 3, 4, "…", totalPages];
    if (page >= totalPages - 2) return [1, "…", totalPages - 3, totalPages - 2, totalPages - 1, totalPages];

    return [1, "…", page - 1, page, page + 1, "…", totalPages];
}