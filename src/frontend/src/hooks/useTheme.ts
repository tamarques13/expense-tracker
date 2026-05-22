import { useEffect, useState } from "react";
import type { ThemeType } from "../types/core";

export function useTheme() {
    const [theme, setTheme] = useState<ThemeType>("light");

    useEffect(() => { document.documentElement.setAttribute("data-theme", theme) }, [theme]);

    return { theme, toggle: () => setTheme((t) => (t === "light" ? "dark" : "light")) };
}