import { useEffect, useMemo, useState } from 'react';
import { Link } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { getDomainPerformances } from '../services/progressService';
import Icon from './Icon';
import { formatDate } from '../utils/userDisplay';
import { ProgressSkeleton, ContentReveal } from './loading';

export default function LearnerProgressDashboard({ showWelcome = true }) {
  const { user } = useAuth();
  const [performances, setPerformances] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    if (!user?.userId) return;
    setLoading(true);
    getDomainPerformances()
      .then(setPerformances)
      .catch(() => setError('Failed to load progress data.'))
      .finally(() => setLoading(false));
  }, [user?.userId]);

  const groupedByExam = useMemo(() => {
    const map = new Map();
    performances.forEach((row) => {
      const key = row.examId;
      if (!map.has(key)) {
        map.set(key, { examTitle: row.examTitle, domains: [] });
      }
      map.get(key).domains.push(row);
    });
    return [...map.entries()];
  }, [performances]);

  const displayName = user?.displayName || user?.userName || 'there';

  return (
    <>
      {showWelcome && (
        <div className="mb-xl">
          <h1 className="font-headline-xl text-headline-xl text-primary mb-xs">
            Welcome back, {displayName}
          </h1>
          <p className="text-on-surface-variant max-w-2xl">
            Domain-level performance from completed exam sessions. Finish an exam to see scores
            here.
          </p>
        </div>
      )}

      {loading && <ProgressSkeleton />}
      {error && <p className="text-red-600 loading-enter">{error}</p>}

      <ContentReveal show={!loading && !error && performances.length === 0}>
        <div className="bg-white rounded-xl border border-outline-variant p-xl text-center">
          <Icon name="query_stats" className="text-primary mb-md" style={{ fontSize: 48 }} />
          <h2 className="font-headline-md text-headline-md font-bold mb-sm">No progress yet</h2>
          <p className="text-on-surface-variant mb-lg">
            Complete an exam session to track your performance by domain.
          </p>
          <Link
            to="/exams"
            className="inline-block bg-secondary-container text-white px-lg py-sm rounded-lg font-bold"
          >
            Browse exams
          </Link>
        </div>
      </ContentReveal>

      <ContentReveal show={!loading && groupedByExam.length > 0}>
        <div className="space-y-lg">
          {groupedByExam.map(([examId, { examTitle, domains }]) => (
            <section
              key={examId}
              className="bg-white rounded-xl border border-outline-variant shadow-sm overflow-hidden"
            >
              <div className="p-lg border-b border-outline-variant">
                <h2 className="font-headline-sm text-headline-sm font-bold">{examTitle}</h2>
              </div>
              <div className="overflow-x-auto">
                <table className="w-full text-left text-sm">
                  <thead className="bg-surface-container-low text-on-surface-variant text-xs uppercase">
                    <tr>
                      <th className="px-lg py-md">Domain</th>
                      <th className="px-lg py-md">Score</th>
                      <th className="px-lg py-md">Correct</th>
                      <th className="px-lg py-md">Answered</th>
                      <th className="px-lg py-md">Last updated</th>
                    </tr>
                  </thead>
                  <tbody className="divide-y divide-outline-variant">
                    {domains.map((row) => (
                      <tr key={row.domainId}>
                        <td className="px-lg py-md font-medium">{row.domainTitle}</td>
                        <td className="px-lg py-md">
                          <span className="font-bold text-primary">
                            {Number(row.percentageScore).toFixed(1)}%
                          </span>
                        </td>
                        <td className="px-lg py-md">{Number(row.totalCorrect).toFixed(1)}</td>
                        <td className="px-lg py-md">{row.totalAnswered}</td>
                        <td className="px-lg py-md text-on-surface-variant">
                          {formatDate(row.lastUpdated)}
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            </section>
          ))}
        </div>
      </ContentReveal>
    </>
  );
}
