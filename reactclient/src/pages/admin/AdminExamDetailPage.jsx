import { useEffect, useState } from 'react';
import { Link, useNavigate, useParams } from 'react-router-dom';
import AdminLayout from '../../components/admin/AdminLayout';
import StatusBadge from '../../components/admin/StatusBadge';
import Icon from '../../components/Icon';
import { getExamDetails, publishExam, getAllExams } from '../../services/adminExamService';
import { getDomainsByExam } from '../../services/adminDomainService';
import { formatApiErrors } from '../../services/authService';
import { formatExamStatus, statusBadgeType } from '../../utils/examStatus';
import { DetailPageSkeleton } from '../../components/loading';

export default function AdminExamDetailPage() {
  const { examId } = useParams();
  const navigate = useNavigate();
  const [exam, setExam] = useState(null);
  const [examMeta, setExamMeta] = useState(null);
  const [domains, setDomains] = useState([]);
  const [loading, setLoading] = useState(true);
  const [publishing, setPublishing] = useState(false);
  const [actionError, setActionError] = useState(null);

  useEffect(() => {
    let cancelled = false;

    async function load() {
      try {
        const [details, domainList, allExams] = await Promise.all([
          getExamDetails(examId),
          getDomainsByExam(examId),
          getAllExams(),
        ]);
        if (!cancelled) {
          setExam(details);
          setDomains(domainList || []);
          setExamMeta(allExams.find((e) => e.id === examId) || null);
        }
      } catch {
        if (!cancelled) setExam(null);
      } finally {
        if (!cancelled) setLoading(false);
      }
    }

    load();
    return () => {
      cancelled = true;
    };
  }, [examId]);

  const handlePublish = async () => {
    setPublishing(true);
    setActionError(null);
    try {
      await publishExam(examId);
      const allExams = await getAllExams();
      setExamMeta(allExams.find((e) => e.id === examId) || null);
    } catch (err) {
      setActionError(formatApiErrors(err));
    } finally {
      setPublishing(false);
    }
  };

  if (loading) {
    return (
      <AdminLayout title="Exam Details">
        <DetailPageSkeleton />
      </AdminLayout>
    );
  }

  if (!exam) {
    return (
      <AdminLayout title="Exam Details">
        <p className="text-red-600">Exam not found.</p>
        <Link to="/admin/exams" className="text-secondary-container font-bold hover:underline">
          Back to exams
        </Link>
      </AdminLayout>
    );
  }

  const status = examMeta?.status;

  return (
    <AdminLayout title={exam.title || 'Exam Details'}>
      <button
        type="button"
        onClick={() => navigate('/admin/exams')}
        className="mb-lg text-sm text-secondary-container font-bold hover:underline flex items-center gap-xs"
      >
        <Icon name="arrow_back" style={{ fontSize: 18 }} />
        Back to exams
      </button>

      {actionError && (
        <div className="mb-lg p-md bg-red-50 text-red-700 rounded-lg border border-red-200 flex justify-between gap-md">
          <span>{actionError}</span>
          <button type="button" onClick={() => setActionError(null)} className="shrink-0 font-bold">
            Dismiss
          </button>
        </div>
      )}

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-lg">
        <div className="lg:col-span-2 space-y-lg">
          <div className="bg-white rounded-xl border border-outline-variant shadow-sm p-lg">
            <div className="flex flex-wrap items-center gap-md mb-md">
              {status !== undefined && (
                <StatusBadge status={formatExamStatus(status)} type={statusBadgeType(status)} />
              )}
            </div>
            <p className="text-on-surface-variant mb-lg">{exam.context || 'No description.'}</p>
            <dl className="grid grid-cols-2 gap-md text-sm">
              <div>
                <dt className="text-on-surface-variant">Duration</dt>
                <dd className="font-bold">{exam.durationInMinutes} minutes</dd>
              </div>
              <div>
                <dt className="text-on-surface-variant">Questions (target)</dt>
                <dd className="font-bold">{exam.numberOfQuestions}</dd>
              </div>
            </dl>
            <div className="flex flex-wrap gap-md mt-lg pt-lg border-t border-outline-variant">
              <Link
                to={`/admin/exams/${examId}/edit`}
                className="px-md py-sm rounded-lg border border-outline-variant font-bold text-sm hover:bg-surface-container-low"
              >
                Edit duration & settings
              </Link>
              {formatExamStatus(status) !== 'Published' && (
                <button
                  type="button"
                  onClick={handlePublish}
                  disabled={publishing}
                  className="px-md py-sm rounded-lg bg-green-600 text-white font-bold text-sm disabled:opacity-50"
                >
                  {publishing ? 'Publishing…' : 'Publish Exam'}
                </button>
              )}
            </div>
          </div>

          <div className="bg-white rounded-xl border border-outline-variant shadow-sm p-lg">
            <h2 className="font-headline-sm text-headline-sm font-bold mb-md">Domains</h2>
            {domains.length === 0 ? (
              <p className="text-on-surface-variant text-sm">No domains found.</p>
            ) : (
              <ul className="divide-y divide-outline-variant">
                {domains.map((d) => (
                  <li key={d.id} className="py-md flex justify-between">
                    <div>
                      <p className="font-medium">{d.title}</p>
                      <p className="text-sm text-on-surface-variant">{d.description}</p>
                    </div>
                    <span className="text-sm text-on-surface-variant">Weight: {d.weight}</span>
                  </li>
                ))}
              </ul>
            )}
          </div>
        </div>

        <div className="space-y-lg">
          <div className="bg-white rounded-xl border border-outline-variant shadow-sm p-lg">
            <h2 className="font-headline-sm text-headline-sm font-bold mb-md">Question Pool</h2>
            <p className="text-sm text-on-surface-variant mb-md">
              Questions are managed in the global pool and sampled by domain when learners start a
              session.
            </p>
            <Link
              to="/admin/questions"
              className="w-full flex items-center justify-center gap-sm px-md py-sm rounded-lg bg-secondary-container text-white font-bold text-sm hover:brightness-110"
            >
              <Icon name="library_books" style={{ fontSize: 18 }} />
              Open Question Pool
            </Link>
          </div>
        </div>
      </div>
    </AdminLayout>
  );
}
