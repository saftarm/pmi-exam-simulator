import Icon from './Icon';

export default function QuestionMap({
    total,
    currentIndex,
    answers,
    flagged,
    onSelect,
}) {
    return (
        <aside className="w-80 bg-white border-r border-outline-variant flex flex-col shrink-0">
            <div className="p-md border-b border-outline-variant">
                <h3 className="font-label-lg text-label-lg text-on-surface-variant flex items-center gap-sm">
                    <Icon name="grid_view" className="text-md" />
                    QUESTION MAP
                </h3>
            </div>
            <div className="flex-1 overflow-y-auto p-md custom-scrollbar">
                <div className="grid grid-cols-5 gap-sm">
                    {Array.from({ length: total }, (_, i) => {
                        const num = i + 1;
                        const isCurrent = i === currentIndex;
                        const isAnswered = answers[i] != null;
                        const isFlagged = flagged.has(i);

                        let statusClass = 'bg-surface-container-low text-on-surface-variant border-transparent';
                        if (isCurrent) {
                            statusClass = 'bg-primary text-white border-primary shadow-lg ring-2 ring-primary/20';
                        } else if (isFlagged) {
                            statusClass = 'bg-white border-primary text-primary border-dashed border-2';
                        } else if (isAnswered) {
                            statusClass = 'bg-primary-fixed text-primary border-primary-fixed';
                        }

                        return (
                            <button
                                key={num}
                                type="button"
                                onClick={() => onSelect(i)}
                                className={`h-10 w-10 flex items-center justify-center font-label-sm rounded-lg border transition-all hover:border-primary ${statusClass}`}
                            >
                                {num}
                            </button>
                        );
                    })}
                </div>
            </div>
            <div className="p-md border-t border-outline-variant bg-surface-container-low">
                <div className="flex flex-col gap-sm">
                    <div className="flex items-center justify-between font-label-sm text-on-surface-variant">
                        <span className="flex items-center gap-xs">
                            <span className="w-3 h-3 bg-primary-fixed rounded-full" />
                            Completed
                        </span>
                        <span className="flex items-center gap-xs">
                            <span className="w-3 h-3 bg-white border-2 border-primary border-dashed rounded-full" />
                            Flagged
                        </span>
                    </div>
                </div>
            </div>
        </aside>
    );
}
