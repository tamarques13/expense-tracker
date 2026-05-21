export interface MonthAnalytics {
    year: number;
    month: number;
    amounts: MonthAmounts;
    spendingCategory: SpendingCategory;
    trend?: TrendAnalytics | null;
    categories: CategoryAnalytics[];
}

export interface MonthAmounts {
    total: number;
    averageDaily: number;
    median: number;
    largest: number;
}

export interface TrendAnalytics {
    previousMonthTotal?: number | null;
    monthChange: number;
    monthPercentageChange: number;
    multiplier: number;
    isImproving: boolean;
}

export interface CategoryAnalytics {
    categoryName: string;
    category: string;
    total: number;
    transactionsNum: number;
    average: number;
    percentage: number;
    previousMonthTotal?: number | null;
    lastMonthChange?: number | null;
}

export interface SpendingCategory {
    highest: string;
    lowest: string;

}