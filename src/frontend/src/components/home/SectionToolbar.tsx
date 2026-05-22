import { PlusIcon } from "../Icons/ButtonIcons";

interface SectionToolbarProps {
    count: number;
    onAdd: () => void;
}

export function SectionToolbar({ count, onAdd }: SectionToolbarProps) {

    return (
        <div className="section-toolbar">
            <div className="section-toolbar__left">
                <span className="section-title">Recent</span>
                <span className="count-badge">{count} items</span>
            </div>

            <button className="btn btn--primary" onClick={onAdd}>
                <PlusIcon />
                Add expense
            </button>
        </div>
    );
}
