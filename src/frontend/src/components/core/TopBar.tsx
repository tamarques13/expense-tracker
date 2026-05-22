import { Link } from "react-router-dom";

import type { ThemeType } from "../../types/core";

import { ReceiptIcon, SunIcon, MoonIcon } from "../Icons/TopBarIcons";

interface TopBarProps {
  theme: ThemeType;
  toggle: () => void;
}

export function TopBar({ theme, toggle }: TopBarProps) {
  return (
    <header className="topbar">
      <Link to="/" className="topbar__brand">
        <ReceiptIcon />
        Expense Tracker
      </Link>

      <div className="topbar__right">
        <button className="btn btn--ghost btn--icon" onClick={toggle} aria-label={theme === "dark" ? "Switch to light mode" : "Switch to dark mode"} >
          {theme === "dark" ? <SunIcon /> : <MoonIcon />}
        </button>
      </div>
    </header>
  );
}