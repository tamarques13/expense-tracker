import { useEffect, useState } from "react";

export function useTheme() {
    const [theme, setTheme] = useState<"light" | "dark">("light");

    useEffect(() => {
        document.documentElement.setAttribute("data-theme", theme);
    }, [theme]);

    return {
        theme,
        toggle: () =>
            setTheme((t) => (t === "light" ? "dark" : "light")),
    };
}