import { useEffect, useState } from 'react';
import Icon from './Icon';
import LoadingButton from './loading/LoadingButton';

function formatTime(totalSeconds) {
  const mins = Math.floor(totalSeconds / 60);
  const secs = totalSeconds % 60;
  return `${mins}:${secs.toString().padStart(2, '0')}`;
}

export default function SessionTimer({ durationMinutes = 230, onExpire }) {
  const [seconds, setSeconds] = useState(durationMinutes * 60);
  const isWarning = seconds < 600;

  useEffect(() => {
    const id = setInterval(() => {
      setSeconds((s) => {
        if (s <= 1) {
          onExpire?.();
          return 0;
        }
        return s - 1;
      });
    }, 1000);
    return () => clearInterval(id);
  }, [onExpire]);

  return (
    <div className="flex items-center gap-sm">
      <Icon name="timer" className="text-secondary-container" />
      <span
        className={`font-mono text-headline-md font-semibold ${isWarning ? 'text-error animate-pulse' : 'text-secondary-container'}`}
      >
        {formatTime(seconds)}
      </span>
    </div>
  );
}

export function SessionHeader({
  durationMinutes,
  currentIndex,
  totalQuestions,
  onSubmit,
  submitting,
  onExpire,
}) {
  const progress = totalQuestions > 0 ? ((currentIndex + 1) / totalQuestions) * 100 : 0;

  return (
    <header className="bg-primary-container text-white h-16 flex items-center justify-between px-margin-desktop shrink-0 z-50">
      <div className="flex items-center gap-lg">
        <span className="font-headline-md text-headline-md font-bold tracking-tight text-white">
          PMI Exam Simulator
        </span>
        <div className="h-6 w-px bg-on-primary-container/30" />
        <SessionTimer durationMinutes={durationMinutes} onExpire={onExpire} />
      </div>
      <div className="flex items-center gap-lg">
        <div className="hidden md:flex flex-col items-end">
          <span className="font-label-sm text-on-primary-container uppercase tracking-widest">
            Progress
          </span>
          <div className="flex items-center gap-sm">
            <div className="w-48 h-2 bg-on-primary-container/20 rounded-full overflow-hidden">
              <div
                className="h-full bg-secondary-container transition-all duration-300"
                style={{ width: `${progress}%` }}
              />
            </div>
            <span className="font-label-lg text-label-lg">
              {currentIndex + 1} / {totalQuestions}
            </span>
          </div>
        </div>
        <LoadingButton
          onClick={onSubmit}
          loading={submitting}
          loadingText="Submitting…"
          className="bg-secondary-container text-on-secondary-container px-lg py-sm rounded font-label-lg hover:brightness-110 transition-all active:scale-95 shadow-md disabled:opacity-60"
        >
          Submit Exam
        </LoadingButton>
      </div>
    </header>
  );
}
