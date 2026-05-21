import { useEffect, useState } from "react";
import { getMonthAnalytics } from "./api";
import type { MonthAnalytics } from "./types";

export function useAnalytics(date = {}) {
    const [analytics, setAnalytics] = useState<MonthAnalytics>();
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    // Load analytics when date changes
    useEffect(() => {
        async function load() {
            try {
                setLoading(true);
                setError(null);

                const response = await getMonthAnalytics(date);

                setAnalytics(response);

            } catch (err) {
                console.error(err);
                setError("Failed to load expenses");

            } finally {
                setLoading(false);
            }
        }

        load();
    }, [date]);

    return {
        analytics,
        loading,
        error,
    };
}
