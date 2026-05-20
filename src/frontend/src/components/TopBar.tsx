interface TopBarProps {
  theme: "light" | "dark";
  toggle: () => void;
}
 
export function TopBar({ theme, toggle }: TopBarProps) {
  return (
    <header className="topbar">
      <a href="/" className="topbar__brand">
        {/* Receipt icon */}
        <svg
          width="18"
          height="18"
          viewBox="0 0 18 18"
          fill="none"
          stroke="currentColor"
          strokeWidth="1.6"
          strokeLinecap="round"
          strokeLinejoin="round"
          aria-hidden="true"
        >
          <path d="M3 1h12v16l-2-1.5-2 1.5-2-1.5-2 1.5-2-1.5V1z" />
          <path d="M6 6h6M6 9h6M6 12h4" />
        </svg>
        Spendly
      </a>
 
      <div className="topbar__right">
        <button
          className="btn btn--ghost btn--icon"
          onClick={toggle}
          aria-label={theme === "dark" ? "Switch to light mode" : "Switch to dark mode"}
        >
          {theme === "dark" ? (
            // Sun icon
            <svg width="16" height="16" viewBox="0 0 16 16" fill="none" stroke="currentColor" strokeWidth="1.8" strokeLinecap="round" aria-hidden="true">
              <circle cx="8" cy="8" r="3" />
              <path d="M8 1v1.5M8 13.5V15M1 8h1.5M13.5 8H15M3.05 3.05l1.06 1.06M11.9 11.9l1.05 1.05M3.05 12.95l1.06-1.06M11.9 4.1l1.05-1.05" />
            </svg>
          ) : (
            // Moon icon
            <svg width="16" height="16" viewBox="0 0 16 16" fill="none" stroke="currentColor" strokeWidth="1.8" strokeLinecap="round" aria-hidden="true">
              <path d="M13.5 10A6 6 0 1 1 6 2.5a4.5 4.5 0 0 0 7.5 7.5z" />
            </svg>
          )}
        </button>
      </div>
    </header>
  );
}