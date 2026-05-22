import { useEffect, useRef } from "react";

interface ModalProps {
  open: boolean;
  onClose: () => void;
  children: React.ReactNode;
}

export function Modal({ open, onClose, children }: ModalProps) {
  const dialogRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    if (!open) return;
    function handleKeyDown(e: KeyboardEvent) { if (e.key === "Escape") onClose() }
    document.addEventListener("keydown", handleKeyDown);

    return () => document.removeEventListener("keydown", handleKeyDown);
  }, [open, onClose]);

  useEffect(() => { if (open) dialogRef.current?.focus() }, [open]);

  if (!open) return null;

  function handleBackdropClick(e: React.MouseEvent<HTMLDivElement>) {
    if (e.target === e.currentTarget) onClose();
  }

  return (
    <div
      className="modal-backdrop"
      role="dialog"
      aria-modal="true"
      onClick={handleBackdropClick}
    >
      <div ref={dialogRef} className="modal" tabIndex={-1}>
        {children}
      </div>
    </div>
  );
}