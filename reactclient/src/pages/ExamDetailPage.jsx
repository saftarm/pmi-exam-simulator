import { useEffect, useState } from 'react';
import { Link, useNavigate, useParams } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { getExamDetails, startSession } from '../services/examService';
import { getDomainsByExam } from '../services/adminDomainService';
import AppHeader from '../components/AppHeader';
import AppFooter from '../components/AppFooter';
import Icon from '../components/Icon';
import { DetailPageSkeleton, ContentReveal, LoadingButton } from '../components/loading';

export default function ExamDetailPage() {
    const { examId } = useParams();
    const navigate = useNavigate();
    const { user } = useAuth();
    const [exam, setExam] = useState(null);
    const [domains, setDomains] = useState([]);
    const [loading, setLoading] = useState(true);
    const [starting, setStarting] = useState(false);
    const [error, setError] = useState(null);

    useEffect(() => {
        Promise.all([
            getExamDetails(examId),
            getDomainsByExam(examId).catch(() => []),
        ])
            .then(([details, domainList]) => {
                setExam(details);
                setDomains(domainList || []);
            })
            .catch(() => setError('Exam not found or unavailable.'))
            .finally(() => setLoading(false));
    }, [examId]);

    const handleStart = async () => {
        if (!user?.userId) return;
        setStarting(true);
        try {
            const session = await startSession(user.userId, examId);
            navigate(`/exams/${examId}/session/${session.sessionId}`, {
                state: { exam },
            });
        } catch {
            alert('Could not start exam session. Please try again.');
        } finally {
            setStarting(false);
        }
    };

    return (
        <div className="min-h-screen flex flex-col bg-[#F4F5F7]">
            <AppHeader activeLink="Exams" />

            <main className="flex-1 max-w-container-max mx-auto px-margin-desktop py-xl w-full">
                <Link
                    to="/exams"
                    className="mb-lg text-sm text-secondary-container font-bold hover:underline flex items-center gap-xs"
                >
                    <Icon name="arrow_back" style={{ fontSize: 18 }} />
                    Back to exams
                </Link>

                {loading && <DetailPageSkeleton />}

                {error && (
                    <p className="text-red-600 loading-enter">{error}</p>
                )}

                <ContentReveal show={!!exam && !error}>
                    <div className="grid grid-cols-1 lg:grid-cols-3 gap-lg">
                        <div className="lg:col-span-2 bg-white rounded-xl border border-outline-variant shadow-sm p-xl">
                            <h1 className="font-headline-xl text-headline-xl text-primary mb-md">{exam.title}</h1>
                            <p className="text-on-surface-variant mb-xl">{exam.context || 'PMI certification exam simulation.'}</p>
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
                            <h2 className="font-headline-sm text-headline-sm font-bold mb-md">Performance domains</h2>
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
            </main>

            <AppFooter />
        </div>
    );
}
