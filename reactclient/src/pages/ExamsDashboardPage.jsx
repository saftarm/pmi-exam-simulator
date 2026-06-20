import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { getPublishedExams, startSession } from '../services/examService';
import AppHeader from '../components/AppHeader';
import AppFooter from '../components/AppFooter';
import ExamCard from '../components/ExamCard';
import Icon from '../components/Icon';

export default function ExamsDashboardPage() {
    const navigate = useNavigate();
    const { user } = useAuth();

    const [exams, setExams] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [startingId, setStartingId] = useState(null);

    useEffect(() => {
        let cancelled = false;

        async function fetchExams() {
            try {
                const data = await getPublishedExams();
                if (!cancelled) setExams(data);
            } catch (err) {
                if (!cancelled) setError(err.message || 'Failed to load exams');
            } finally {
                if (!cancelled) setLoading(false);
            }
        }

        fetchExams();
        return () => { cancelled = true; };
    }, []);

    const handleStart = async (examId) => {
        if (!user?.userId) return;
        setStartingId(examId);
        try {
            const session = await startSession(user.userId, examId);
            navigate(`/exams/${examId}/session/${session.sessionId}`, {
                state: { questions: session.questions, exam: exams.find((e) => e.id === examId) },
            });
        } catch (err) {
            console.error('Failed to start session:', err);
            alert('Could not start exam session. Please try again.');
        } finally {
            setStartingId(null);
        }
    };

    const totalQuestions = exams.reduce((a, e) => a + (e.numberOfQuestions ?? 0), 0);

    return (
        <div className="font-body-md text-on-surface antialiased min-h-screen flex flex-col bg-[#F4F5F7]">
            <AppHeader activeLink="Exams" />

            <main className="max-w-container-max mx-auto px-margin-desktop py-xl flex-1 w-full">
                <div className="mb-xl flex flex-col md:flex-row justify-between items-end gap-md">
                    <div>
                        <h1 className="font-headline-xl text-headline-xl text-primary mb-xs">Available Exams</h1>
                        <p className="font-body-lg text-body-lg text-on-surface-variant max-w-2xl">
                            Select a certification exam to launch the simulation. These exams are meticulously crafted to match the current PMI performance domains.
                        </p>
                    </div>
                    <div className="flex gap-sm">
                        <div className="bg-surface border border-outline-variant px-md py-sm rounded flex items-center gap-xs">
                            <Icon name="filter_list" className="text-primary" style={{ fontSize: 20 }} />
                            <span className="font-label-lg text-label-lg">Filter</span>
                        </div>
                        <div className="bg-surface border border-outline-variant px-md py-sm rounded flex items-center gap-xs">
                            <Icon name="sort" className="text-primary" style={{ fontSize: 20 }} />
                            <span className="font-label-lg text-label-lg">Sort</span>
                        </div>
                    </div>
                </div>

                {loading && (
                    <div className="text-center py-xl font-body-lg text-on-surface-variant">Loading exams...</div>
                )}

                {error && (
                    <div className="bg-error-container text-on-error-container px-lg py-md rounded-lg">{error}</div>
                )}

                {!loading && !error && (
                    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-xl">
                        {exams.map((exam, index) => (
                            <ExamCard
                                key={exam.id}
                                exam={exam}
                                index={index}
                                onStart={handleStart}
                                starting={startingId === exam.id}
                            />
                        ))}
                    </div>
                )}

                {!loading && !error && exams.length === 0 && (
                    <div className="text-center py-xl text-on-surface-variant">No published exams available yet.</div>
                )}

                <section className="mt-xl">
                    <h2 className="font-headline-lg text-headline-lg text-primary mb-lg">Your Performance Trends</h2>
                    <div className="bg-white rounded-xl border border-outline-variant p-xl">
                        <div className="flex flex-col md:flex-row gap-xl items-center">
                            <div className="flex-grow w-full md:w-auto">
                                <div className="flex justify-between items-center mb-md">
                                    <span className="font-label-lg text-label-lg text-primary">Target Performance (80%)</span>
                                    <span className="font-label-lg text-label-lg text-secondary">Current: 74%</span>
                                </div>
                                <div className="w-full bg-surface-container-highest h-3 rounded-full overflow-hidden">
                                    <div className="bg-secondary-container h-full rounded-full" style={{ width: '74%' }} />
                                </div>
                                <div className="mt-md flex gap-xl">
                                    <div className="flex flex-col">
                                        <span className="text-[24px] font-bold text-primary">{exams.length}</span>
                                        <span className="font-label-sm text-label-sm text-on-surface-variant">Exams Available</span>
                                    </div>
                                    <div className="flex flex-col">
                                        <span className="text-[24px] font-bold text-primary">{totalQuestions}</span>
                                        <span className="font-label-sm text-label-sm text-on-surface-variant">Questions Answered</span>
                                    </div>
                                    <div className="flex flex-col">
                                        <span className="text-[24px] font-bold text-primary">—</span>
                                        <span className="font-label-sm text-label-sm text-on-surface-variant">Study Time</span>
                                    </div>
                                </div>
                            </div>
                            <div className="w-full md:w-64 bg-surface-container-low p-lg rounded-lg border border-outline-variant">
                                <p className="font-label-sm text-label-sm text-on-surface-variant uppercase tracking-wider mb-sm">Next Suggested Session</p>
                                <p className="font-headline-md text-headline-md text-primary mb-lg">
                                    {exams[0]?.title ?? 'PMP® Domain II: Process'}
                                </p>
                                {exams[0] && (
                                    <button
                                        type="button"
                                        onClick={() => handleStart(exams[0].id)}
                                        className="text-secondary font-label-lg text-label-lg flex items-center gap-xs hover:underline"
                                    >
                                        Start Review
                                        <Icon name="arrow_forward" style={{ fontSize: 16 }} />
                                    </button>
                                )}
                            </div>
                        </div>
                    </div>
                </section>
            </main>

            <AppFooter />
        </div>
    );
}
