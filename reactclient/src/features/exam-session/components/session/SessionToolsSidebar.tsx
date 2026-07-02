import Icon from '../../../../shared/components/Icon';
import { countAnswered } from '../../utils/sessionAnswers';
import type { SessionAnswersMap } from '../../utils/sessionAnswers';

interface SessionToolsSidebarProps {
  answers: SessionAnswersMap;
  total: number;
  flaggedCount: number;
}

export default function SessionToolsSidebar({
  answers,
  total,
  flaggedCount,
}: SessionToolsSidebarProps) {
  return (
    <aside className="hidden xl:flex w-64 bg-white border-l border-outline-variant p-lg flex-col gap-xl shrink-0">
      <div>
        <h4 className="font-label-lg text-label-lg text-on-surface-variant mb-md uppercase tracking-wider">
          Exam Stats
        </h4>
        <div className="space-y-lg">
          <div className="bg-surface-container-low p-md rounded-lg">
            <div className="flex items-center justify-between mb-xs">
              <span className="text-on-surface-variant font-label-sm">ANSWERED</span>
              <span className="font-headline-md text-primary">
                {countAnswered(answers)} / {total}
              </span>
            </div>
            <div className="w-full h-1 bg-outline-variant rounded-full">
              <div
                className="h-full bg-primary rounded-full transition-all"
                style={{
                  width: `${total ? (countAnswered(answers) / total) * 100 : 0}%`,
                }}
              />
            </div>
          </div>
          <div className="bg-surface-container-low p-md rounded-lg">
            <div className="flex items-center justify-between">
              <span className="text-on-surface-variant font-label-sm">FLAGGED</span>
              <span className="font-headline-md text-secondary">{flaggedCount}</span>
            </div>
          </div>
        </div>
      </div>
      <div className="flex-1 flex flex-col">
        <h4 className="font-label-lg text-label-lg text-on-surface-variant mb-md uppercase tracking-wider">
          Tools
        </h4>
        <div className="grid grid-cols-2 gap-sm">
          {(['calculate', 'draw', 'translate', 'help_center'] as const).map((tool) => (
            <button
              key={tool}
              type="button"
              className="flex flex-col items-center justify-center p-md border border-outline-variant rounded hover:bg-surface-container-low transition-colors group"
            >
              <Icon
                name={tool}
                className="text-primary group-hover:scale-110 transition-transform"
              />
              <span className="font-label-sm mt-xs capitalize">{tool.replace('_', ' ')}</span>
            </button>
          ))}
        </div>
      </div>
      <div className="p-md bg-secondary-fixed text-on-secondary-fixed rounded-lg border border-secondary-fixed-dim">
        <p className="font-label-sm flex items-start gap-xs">
          <Icon name="lightbulb" className="text-sm mt-0.5" />
          <span>
            Remember to use the <strong>Flag for Review</strong> button on questions you want to
            revisit.
          </span>
        </p>
      </div>
    </aside>
  );
}
