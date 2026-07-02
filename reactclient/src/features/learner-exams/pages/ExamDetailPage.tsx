import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { useAuth } from '../../auth';
import { getExamDetails, getDomainsByExam } from '../api';
import type { ExamDetailsDto, LearnerExamDomainDto } from '../types';
import { useStartExamSession } from '../hooks/useStartExamSession';
import BackLink from '../../../shared/components/BackLink';
import Icon from '../../../shared/components/Icon';
import ErrorBanner from '../../../shared/components/ErrorBanner';
import { DetailPageSkeleton, ContentReveal, LoadingButton } from '../../../shared/components/loading';

export default function ExamDetailPage() {
  const { examId = '' } = useParams();
  const { user } = useAuth();
  const { startExam, starting, startError } = useStartExamSession();
  const [exam, setExam] = useState<ExamDetailsDto | null>(null);
  const [domains, setDomains] = useState<LearnerExamDomainDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    Promise.all([getExamDetails(examId), getDomainsByExam(examId).catch(() => [])])
      .then(([details, domainList]) => {
        setExam(details);
        setDomains(domainList || []);
      })
      .catch(() => setError('Exam not found or unavailable.'))
      .finally(() => setLoading(false));
  }, [examId]);

  const handleStart = async () => {
    if (!user?.userId || !exam) return;
    try {
      await startExam(examId, exam);
    } catch {
      // startError is set by the hook
    }
  };

  return (
    <>
      <BackLink to="/exams" label="Back to exams" />

      {loading && <DetailPageSkeleton />}

      {error && <p className="text-red-600 loading-enter">{error}</p>}

      {exam && !error && (
        <ContentReveal show>
          <div className="grid grid-cols-1 lg:grid-cols-3 gap-lg">
            <div className="lg:col-span-2 bg-white rounded-xl border border-outline-variant shadow-sm p-xl">
              <h1 className="font-headline-xl text-headline-xl text-primary mb-md">{exam.title}</h1>
              {exam.context && <p className="text-on-surface-variant mb-xl">{exam.context}</p>}
              <dl className="grid grid-cols-2 gap-md text-sm mb-xl">
                <div>
                  <dt className="text-on-surface-variant">Duration</dt>
                  <dd className="font-bold text-lg">{exam.durationInMinutes} minutes</dd>
                </div>
                <div>
                  <dt className="text-on-surface-variant">Questions</dt>
                  <dd className="font-bold text-lg">{exam.numberOfQuestions}</dd>
                </div>
              </dl>
              <ErrorBanner message={startError} />
              <LoadingButton
                onClick={handleStart}
                loading={starting}
                loadingText="Launching…"
                className="w-full sm:w-auto bg-secondary-container text-white px-xl py-md rounded-lg font-bold disabled:opacity-60"
              >
                Launch Simulator
                {!starting && <Icon name="play_arrow" style={{ fontSize: 22 }} />}
              </LoadingButton>
            </div>

            <div className="bg-white rounded-xl border border-outline-variant shadow-sm p-lg">
              <h2 className="font-headline-sm text-headline-sm font-bold mb-md">
                Performance domains
              </h2>
              {domains.length === 0 ? (
                <p className="text-sm text-on-surface-variant">No domain breakdown available.</p>
              ) : (
                <ul className="divide-y divide-outline-variant">
                  {domains.map((d) => (
                    <li key={d.id} className="py-md">
                      <p className="font-medium">{d.title}</p>
                      {d.description && (
                        <p className="text-sm text-on-surface-variant mt-xs">{d.description}</p>
                      )}
                    </li>
                  ))}
                </ul>
              )}
            </div>
          </div>
        </ContentReveal>
      )}
    </>
  );
}
