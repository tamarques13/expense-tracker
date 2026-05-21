import { http } from "../../api/http";
import type { MonthAnalytics } from "./types";

export const getMonthAnalytics = async (date = {}): Promise<MonthAnalytics> => {
    const res = await http.get("/analytics/month", { params: { date } });

    return res.data;
};