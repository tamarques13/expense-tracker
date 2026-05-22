interface HeaderProps {
    monthLabel: string;
}

export function Header({ monthLabel }: HeaderProps) {
    return (
        <div className="page-header">
            <p className="page-eyebrow">{monthLabel}</p>
            <h1 className="page-title">Expenses</h1>
        </div>
    )
}