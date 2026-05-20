export default function ThemeToggle({ theme, toggle, }: { theme: string; toggle: () => void; }) {
    return (
        <button className="btn" onClick={toggle}>
            {theme === "light" ? "🌙 Dark" : "☀️ Light"}
        </button>
    );
}