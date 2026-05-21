interface MetricCardProps {
  label: string;
  value: string;
  sub?: React.ReactNode;
}

export function MetricCard({ label, value, sub }: MetricCardProps) {
  return (
    <div className="metric-card">
      <p className="metric-card__label">{label}</p>
      <p className="metric-card__value">{value}</p>
      {sub && <div className="metric-card__sub">{sub}</div>}
    </div>
  );
}