import { http } from "../../api/http";

export const getExpenses = async () => {
    const res = await http.get("/expenses");
    return res.data;
};