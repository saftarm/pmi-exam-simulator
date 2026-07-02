import { Link } from 'react-router-dom';
import Icon from '../../../shared/components/Icon';
import LoadingButton from '../../../shared/components/loading/LoadingButton';
import type { ApiId } from '../../../shared/api/primitives';
import type { ExamDetailsDto } from '../types';

interface ExamCardProps {
  exam: ExamDetailsDto;
  onStart: (examId: ApiId) => void;
  starting: boolean;
}

interface MetaItem {
  icon: string;
  label: string;
}

export default function ExamCard({ exam, onStart, starting }: ExamCardProps) {
  const metaItems: MetaItem[] = [
    { icon: 'quiz', label: `${exam.numberOfQuestions ?? 0} Questions` },
    { icon: 'schedule', label: `${exam.durationInMinutes ?? 0} Mins` },
  ];

  if (exam.attemptCount > 0) {
    metaItems.push({ icon: 'groups', label: `${exam.attemptCount} attempts` });
  }

  return (
    <div className="bg-white rounded-xl border border-outline-variant overflow-hidden flex flex-col card-hover transition-all duration-300">
      <div className="h-48 relative overflow-hidden bg-primary-container flex items-center justify-center">
        <Icon name="school" className="text-primary/30" style={{ fontSize: 72 }} />
        {exam.isMostPopular && (
          <div className="absolute top-md left-md">
            <span className="text-label-sm font-label-sm px-md py-xs rounded-full bg-secondary-container text-on-secondary">
              MOST POPULAR
            </span>
          </div>
        )}
      </div>
      <div className="p-lg flex flex-col flex-grow">
        <h2 className="font-headline-lg text-headline-lg text-primary mb-xs">{exam.title}</h2>
        {exam.context && (
          <p className="font-label-lg text-label-lg text-on-surface-variant mb-lg">
            {exam.context}
          </p>
        )}
        <div className="grid grid-cols-2 gap-md mb-xl">
          {metaItems.map((item) => (
            <div key={item.label} className="flex items-center gap-xs text-on-surface-variant">
              <Icon name={item.icon} style={{ fontSize: 18 }} />
              <span className="font-label-sm text-label-sm">{item.label}</span>
            </div>
          ))}
        </div>
        <div className="mt-auto pt-lg border-t border-outline-variant space-y-sm">
          <Link
            to={`/exams/${exam.id}`}
            className="w-full block text-center py-sm rounded-lg border border-outline-variant font-label-lg text-label-lg text-primary hover:bg-surface-container-low transition-colors"
          >
            View details
          </Link>
          <LoadingButton
            onClick={() => onStart(exam.id)}
            loading={starting}
            loadingText="Launching…"
            className="w-full bg-secondary-container hover:brightness-110 active:scale-[0.98] transition-all text-on-secondary font-label-lg text-label-lg py-md rounded-lg disabled:opacity-60"
          >
            Launch Simulator
            {!starting && <Icon name="play_arrow" style={{ fontSize: 20 }} />}
          </LoadingButton>
        </div>
      </div>
    </div>
  );
}
