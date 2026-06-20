import { useState, useEffect, useMemo } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { getPublishedExams, startSession } from '../services/examService';
import AppHeader from '../components/AppHeader';
import AppFooter from '../components/AppFooter';
import ExamCard from '../components/ExamCard';
import Icon from '../components/Icon';
import { ExamCardSkeleton, ContentReveal } from '../components/loading';

const SORT_OPTIONS = [
    { value: 'title-asc', label: 'Title (A–Z)' },
    { value: 'title-desc', label: 'Title (Z–A)' },
    { value: 'duration', label: 'Duration (shortest)' },
    { value: 'questions', label: 'Questions (most)' },
];

export default function ExamsDashboardPage() {
    const navigate = useNavigate();
    const { user } = useAuth();

    const [exams, setExams] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [startingId, setStartingId] = useState(null);
    const [searchFilter, setSearchFilter] = useState('');
    const [sortBy, setSortBy] = useState('title-asc');
    const [showFilters, setShowFilters] = useState(false);

    useEffect(() => {
        let cancelled = false;

        async function fetchExams() {
            try {
                const data = await getPublishedExams();
                if (!cancelled) setExams(Array.isArray(data) ? data : data?.items || []);
            } catch (err) {
                if (!cancelled) setError(err.message || 'Failed to load exams');
            } finally {
                if (!cancelled) setLoading(false);
            }
        }

        fetchExams();
        return () => { cancelled = true; };
    }, []);

    const displayedExams = useMemo(() => {
        let list = [...exams];
        const query = searchFilter.trim().toLowerCase();
        if (query) {
            list = list.filter(
                (e) =>
                    (e.title || '').toLowerCase().includes(query) ||
                    (e.context || '').toLowerCase().includes(query),
            );
        }
        list.sort((a, b) => {
            switch (sortBy) {
                case 'title-desc':
                    return (b.title || '').localeCompare(a.title || '');
                case 'duration':
                    return (a.durationInMinutes || 0) - (b.durationInMinutes || 0);
                case 'questions':
                    return (b.numberOfQuestions || 0) - (a.numberOfQuestions || 0);
                default:
                    return (a.title || '').localeCompare(b.title || '');
            }
        });
        return list;
    }, [exams, searchFilter, sortBy]);

    const handleStart = async (examId) => {
        if (!user?.userId) return;
        setStartingId(examId);
        try {
            const session = await startSession(user.userId, examId);
            const exam = exams.find((e) => e.id === examId);
            navigate(`/exams/${examId}/session/${session.sessionId}`, {
                state: { exam },
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
                        <button
                            type="button"
                            onClick={() => setShowFilters((v) => !v)}
                            className={`bg-surface border px-md py-sm rounded flex items-center gap-xs transition-colors ${
                                showFilters || searchFilter ? 'border-primary' : 'border-outline-variant'
                            }`}
                        >
                            <Icon name="filter_list" className="text-primary" style={{ fontSize: 20 }} />
                            <span className="font-label-lg text-label-lg">Filter</span>
                        </button>
                        <select
                            value={sortBy}
                            onChange={(e) => setSortBy(e.target.value)}
                            className="bg-surface border border-outline-variant px-md py-sm rounded flex items-center gap-xs font-label-lg text-label-lg"
                            aria-label="Sort exams"
                        >
                            {SORT_OPTIONS.map((opt) => (
                                <option key={opt.value} value={opt.value}>
                                    Sort: {opt.label}
                                </option>
                            ))}
                        </select>
                    </div>
                </div>

                {showFilters && (
                    <div className="mb-lg">
                        <input
                            type="search"
                            placeholder="Filter by title or description…"
                            value={searchFilter}
                            onChange={(e) => setSearchFilter(e.target.value)}
                            className="w-full max-w-md border border-outline-variant rounded-lg px-md py-sm bg-white"
                        />
                    </div>
                )}

                {loading && <ExamCardSkeleton count={3} />}

                {error && (
                    <div className="bg-error-container text-on-error-container px-lg py-md rounded-lg loading-enter">{error}</div>
                )}

                <ContentReveal show={!loading && !error}>
                    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-xl">
                        {displayedExams.map((exam, index) => (
                            <ExamCard
                                key={exam.id}
                                exam={exam}
                                index={index}
                                onStart={handleStart}
                                starting={startingId === exam.id}
                            />
                        ))}
                    </div>
                </ContentReveal>

                <ContentReveal show={!loading && !error && displayedExams.length === 0}>
                    <div className="text-center py-xl text-on-surface-variant">
                        {exams.length === 0
                            ? 'No published exams available yet.'
                            : 'No exams match your filter.'}
                    </div>
                </ContentReveal>

                <section className="mt-xl">
                    <h2 className="font-headline-lg text-headline-lg text-primary mb-lg">Your Performance</h2>
                    <div className="bg-white rounded-xl border border-outline-variant p-xl flex flex-col md:flex-row justify-between items-center gap-lg">
                        <div>
                            <p className="text-on-surface-variant mb-sm">
                                {exams.length} exam{exams.length !== 1 ? 's' : ''} available · {totalQuestions} total questions in catalog
                            </p>
                            <p className="text-sm text-on-surface-variant">
                                Domain scores appear after you complete a session.
                            </p>
                        </div>
                        <Link
                            to="/progress"
                            className="bg-secondary-container text-white px-lg py-md rounded-lg font-bold flex items-center gap-sm hover:brightness-110"
                        >
                            View progress
                            <Icon name="arrow_forward" style={{ fontSize: 18 }} />
                        </Link>
                    </div>
                </section>
            </main>

            <AppFooter />
        </div>
    );
}
