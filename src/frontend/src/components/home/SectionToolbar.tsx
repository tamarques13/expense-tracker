export function SectionToolbar({ count, onAdd }: { count: number; onAdd: () => void }) {
    return (
        <div className="section-toolbar">
            <div className="section-toolbar__left">
                <span className="section-title">Recent</span>
                <span className="count-badge">{count} items</span>
            </div>

            <button className="btn btn--primary" onClick={onAdd}>
                <svg
                    width="14"
                    height="14"
                    viewBox="0 0 14 14"
                    fill="none"
                    stroke="currentColor"
                    strokeWidth="2"
                    strokeLinecap="round"
                    aria-hidden="true"
                >
                    <path d="M7 1v12M1 7h12" />
                </svg>
                Add expense
            </button>
        </div>
    );
}
