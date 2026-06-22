import { useState, useEffect, useCallback } from 'react';
import { Link, useParams, useNavigate, useLocation } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { finishSession } from '../services/examService';
import { getDomainPerformances } from '../services/progressService';
import { loadSessionQuestions, clearSessionQuestions } from '../utils/sessionStorage';
import { SessionHeader } from '../components/SessionTimer';
import QuestionMap from '../components/QuestionMap';
import AppFooter from '../components/AppFooter';
import Icon from '../components/Icon';
import { normalizeQuestionType } from '../utils/questionTypes';
import { ExamSessionSkeleton, LoadingButton } from '../components/loading';

const OPTION_LETTERS = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';

export default function ExamSessionPage() {
    const { sessionId, examId } = useParams();
    const navigate = useNavigate();
    const location = useLocation();
    const { user } = useAuth();

    const examMeta = location.state?.exam;

    const [questions, setQuestions] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [currentIndex, setCurrentIndex] = useState(0);
    const [answers, setAnswers] = useState({});
    const [flagged, setFlagged] = useState(new Set());
    const [submitting, setSubmitting] = useState(false);
    const [finished, setFinished] = useState(false);
    const [result, setResult] = useState(null);
    const [domainResults, setDomainResults] = useState([]);

    const durationMinutes = examMeta?.durationInMinutes ?? 230;

    useEffect(() => {
        const stored = loadSessionQuestions(sessionId);
        if (stored && stored.length > 0) {
            setQuestions(stored);
            setLoading(false);
            return;
        }
        setError('Session questions not found. Please start the exam again.');
        setLoading(false);
    }, [sessionId]);

    const currentQuestion = questions[currentIndex];
    const total = questions.length;
    const selectedOptionId = answers[currentIndex];

    const handleSelectOption = (optionId) => {
        setAnswers((prev) => ({ ...prev, [currentIndex]: optionId }));
    };

    const toggleFlag = () => {
        setFlagged((prev) => {
            const next = new Set(prev);
            if (next.has(currentIndex)) next.delete(currentIndex);
            else next.add(currentIndex);
            return next;
        });
    };

    const handleSubmit = useCallback(async () => {
        if (submitting) return;
        const unanswered = total - Object.keys(answers).length;
        if (unanswered > 0) {
            const ok = window.confirm(`You have ${unanswered} unanswered question(s). Submit anyway?`);
            if (!ok) return;
        }

        setSubmitting(true);
        try {
            const sessionResponses = Object.entries(answers).map(([idx, selectedOptionId]) => ({
                questionId: questions[Number(idx)].questionId,
                selectedOptionId,
            }));
            const res = await finishSession(sessionId, sessionResponses);
            clearSessionQuestions(sessionId);
            setResult(res);
            if (user?.userId) {
                try {
                    const progress = await getDomainPerformances();
                    const targetExamId = examMeta?.id || examId;
                    setDomainResults(
                        (progress || []).filter(
                            (row) => String(row.examId) === String(targetExamId),
                        ),
                    );
                } catch {
                    setDomainResults([]);
                }
            }
            setFinished(true);
        } catch (err) {
            alert(err.message || 'Failed to submit exam');
        } finally {
            setSubmitting(false);
        }
    }, [answers, questions, sessionId, submitting, total, user?.userId, examMeta?.id, examId]);

    const handleExpire = useCallback(() => {
        handleSubmit();
    }, [handleSubmit]);

    if (loading) {
        return <ExamSessionSkeleton />;
    }

    if (error) {
        return (
            <div className="h-screen flex flex-col items-center justify-center gap-md bg-background">
                <p className="text-error font-body-lg">{error}</p>
                <button type="button" onClick={() => navigate('/exams')} className="text-secondary font-label-lg">Back to Exams</button>
            </div>
        );
    }

    if (finished && result) {
        return (
            <div className="min-h-screen bg-[#F4F5F7] flex flex-col">
                <header className="bg-primary-container text-white h-16 flex items-center px-margin-desktop">
                    <span className="font-headline-md text-headline-md font-bold">PMI Exam Simulator</span>
                </header>
                <main className="flex-1 flex items-center justify-center p-xl">
                    <div className="max-w-lg w-full bg-white border border-outline-variant rounded-xl p-xl shadow-sm text-center">
                        <Icon name="celebration" className="text-secondary mb-md" style={{ fontSize: 48 }} />
                        <h1 className="font-headline-lg text-headline-lg text-primary mb-sm">Exam Complete</h1>
                        <p className="font-body-lg text-on-surface-variant mb-xl">
                            You answered {Object.keys(answers).length} of {total} questions.
                        </p>
                        <div className="grid grid-cols-2 gap-md mb-xl">
                            <div className="bg-surface-container-low p-md rounded-lg">
                                <span className="font-headline-xl text-headline-xl text-primary">{result.correctCount}</span>
                                <p className="font-label-sm text-on-surface-variant">Correct</p>
                            </div>
                            <div className="bg-surface-container-low p-md rounded-lg">
                                <span className="font-headline-xl text-headline-xl text-secondary">{result.percentageScore?.toFixed?.(1) ?? result.percentageScore}%</span>
                                <p className="font-label-sm text-on-surface-variant">Score</p>
                            </div>
                        </div>

                        {domainResults.length > 0 && (
                            <div className="mb-xl text-left">
                                <h2 className="font-headline-sm text-headline-sm font-bold mb-md text-center">Performance by domain</h2>
                                <ul className="space-y-sm text-sm">
                                    {domainResults.map((row) => (
                                        <li
                                            key={row.domainId}
                                            className="flex justify-between items-center border border-outline-variant rounded-lg px-md py-sm"
                                        >
                                            <span className="font-medium">{row.domainTitle}</span>
                                            <span className="font-bold text-primary">
                                                {Number(row.percentageScore).toFixed(1)}%
                                            </span>
                                        </li>
                                    ))}
                                </ul>
                            </div>
                        )}

                        <div className="space-y-sm">
                            <button
                                type="button"
                                onClick={() => navigate('/exams')}
                                className="w-full bg-secondary-container text-on-secondary font-label-lg py-md rounded-lg hover:brightness-110 transition-all"
                            >
                                Back to Exams
                            </button>
                            <Link
                                to="/"
                                className="block w-full border border-outline-variant text-primary font-label-lg py-md rounded-lg hover:bg-surface-container-low transition-all"
                            >
                                View full progress
                            </Link>
                        </div>
                    </div>
                </main>
                <AppFooter compact />
            </div>
        );
    }

    if (!currentQuestion) {
        return (
            <div className="h-screen flex items-center justify-center bg-background">
                <p className="text-on-surface-variant">No questions in this session.</p>
            </div>
        );
    }

    const options = currentQuestion.answerOptions ?? currentQuestion.answerOptionsDtos ?? [];
    const isMultipleChoice = normalizeQuestionType(currentQuestion.questionType) === 'MultipleChoice';

    return (
        <div className="bg-background text-on-surface h-screen overflow-hidden flex flex-col">
            <SessionHeader
                durationMinutes={durationMinutes}
                currentIndex={currentIndex}
                totalQuestions={total}
                onSubmit={handleSubmit}
                submitting={submitting}
                onExpire={handleExpire}
            />

            <main className="flex flex-1 overflow-hidden">
                <QuestionMap
                    total={total}
                    currentIndex={currentIndex}
                    answers={answers}
                    flagged={flagged}
                    onSelect={setCurrentIndex}
                />

                <section className="flex-1 overflow-y-auto bg-background p-xl flex flex-col items-center custom-scrollbar">
                    <div className="max-w-[800px] w-full space-y-lg fade-in-up" key={currentIndex}>
                        <div className="flex items-center justify-between">
                            <div>
                                <span className="font-label-lg text-secondary uppercase tracking-widest">
                                    Question {currentIndex + 1} of {total}
                                </span>
                                <h2 className="font-headline-lg text-headline-lg text-primary mt-xs">
                                    {examMeta?.title ?? 'Exam Session'}
                                </h2>
                            </div>
                            <button
                                type="button"
                                onClick={toggleFlag}
                                className={`flex items-center gap-sm px-md py-sm rounded-full border font-label-lg transition-colors ${
                                    flagged.has(currentIndex)
                                        ? 'border-primary bg-primary-fixed text-primary'
                                        : 'border-primary text-primary hover:bg-primary-fixed'
                                }`}
                            >
                                <Icon name="flag" filled={flagged.has(currentIndex)} />
                                Flag for Review
                            </button>
                        </div>

                        <div className="bg-white border border-outline-variant rounded-xl p-xl shadow-sm">
                            {isMultipleChoice && (
                                <div className="mb-md p-md bg-amber-50 border border-amber-200 rounded-lg text-sm text-amber-800">
                                    Multiple selection is not yet supported — select the best single answer.
                                </div>
                            )}
                            <p className="font-body-lg text-body-lg text-on-surface leading-relaxed">
                                {currentQuestion.questionTitle}
                            </p>
                            <div className="mt-xl space-y-md">
                                {options.map((option, i) => {
                                    const letter = OPTION_LETTERS[i] ?? String(i + 1);
                                    const isSelected = selectedOptionId === option.id;
                                    return (
                                        <button
                                            key={option.id}
                                            type="button"
                                            onClick={() => handleSelectOption(option.id)}
                                            className={`group cursor-pointer flex items-start gap-md p-md w-full text-left rounded-xl transition-all ${
                                                isSelected
                                                    ? 'border-2 border-primary bg-primary-fixed/30'
                                                    : 'border border-outline-variant hover:border-primary-fixed hover:bg-surface-container-lowest'
                                            }`}
                                        >
                                            <div className={`mt-1 flex-shrink-0 h-6 w-6 rounded-full border-2 flex items-center justify-center font-label-lg ${
                                                isSelected
                                                    ? 'border-primary bg-primary text-white'
                                                    : 'border-outline-variant text-on-surface-variant group-hover:border-primary-fixed'
                                            }`}
                                            >
                                                {letter}
                                            </div>
                                            <p className="font-body-md text-body-md text-on-surface">{option.text}</p>
                                        </button>
                                    );
                                })}
                            </div>
                        </div>

                        <div className="flex items-center justify-between pt-lg">
                            <button
                                type="button"
                                onClick={() => setCurrentIndex((i) => Math.max(0, i - 1))}
                                disabled={currentIndex === 0}
                                className="flex items-center gap-sm px-xl py-md text-primary font-label-lg hover:bg-primary-fixed rounded-lg transition-colors disabled:opacity-40"
                            >
                                <Icon name="arrow_back" />
                                Previous
                            </button>
                            <div className="flex items-center gap-md">
                                <button
                                    type="button"
                                    onClick={toggleFlag}
                                    className="px-xl py-md bg-white border border-outline-variant text-on-surface-variant font-label-lg rounded-lg hover:bg-surface-container-lowest transition-all"
                                >
                                    Save for later
                                </button>
                                {currentIndex < total - 1 ? (
                                    <button
                                        type="button"
                                        onClick={() => setCurrentIndex((i) => i + 1)}
                                        className="px-xl py-md bg-secondary-container text-on-secondary-container font-label-lg rounded-lg hover:brightness-110 shadow-lg flex items-center gap-sm transition-all active:scale-95"
                                    >
                                        Next Question
                                        <Icon name="arrow_forward" />
                                    </button>
                                ) : (
                                    <LoadingButton
                                        onClick={handleSubmit}
                                        loading={submitting}
                                        loadingText="Submitting…"
                                        className="px-xl py-md bg-secondary-container text-on-secondary-container font-label-lg rounded-lg hover:brightness-110 shadow-lg transition-all active:scale-95 disabled:opacity-60"
                                    >
                                        Submit Exam
                                        {!submitting && <Icon name="check" />}
                                    </LoadingButton>
                                )}
                            </div>
                        </div>
                    </div>
                </section>

                <aside className="hidden xl:flex w-64 bg-white border-l border-outline-variant p-lg flex-col gap-xl shrink-0">
                    <div>
                        <h4 className="font-label-lg text-label-lg text-on-surface-variant mb-md uppercase tracking-wider">Exam Stats</h4>
                        <div className="space-y-lg">
                            <div className="bg-surface-container-low p-md rounded-lg">
                                <div className="flex items-center justify-between mb-xs">
                                    <span className="text-on-surface-variant font-label-sm">ANSWERED</span>
                                    <span className="font-headline-md text-primary">{Object.keys(answers).length} / {total}</span>
                                </div>
                                <div className="w-full h-1 bg-outline-variant rounded-full">
                                    <div
                                        className="h-full bg-primary rounded-full transition-all"
                                        style={{ width: `${total ? (Object.keys(answers).length / total) * 100 : 0}%` }}
                                    />
                                </div>
                            </div>
                            <div className="bg-surface-container-low p-md rounded-lg">
                                <div className="flex items-center justify-between">
                                    <span className="text-on-surface-variant font-label-sm">FLAGGED</span>
                                    <span className="font-headline-md text-secondary">{flagged.size}</span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div className="flex-1 flex flex-col">
                        <h4 className="font-label-lg text-label-lg text-on-surface-variant mb-md uppercase tracking-wider">Tools</h4>
                        <div className="grid grid-cols-2 gap-sm">
                            {['calculate', 'draw', 'translate', 'help_center'].map((tool) => (
                                <button key={tool} type="button" className="flex flex-col items-center justify-center p-md border border-outline-variant rounded hover:bg-surface-container-low transition-colors group">
                                    <Icon name={tool} className="text-primary group-hover:scale-110 transition-transform" />
                                    <span className="font-label-sm mt-xs capitalize">{tool.replace('_', ' ')}</span>
                                </button>
                            ))}
                        </div>
                    </div>
                    <div className="p-md bg-secondary-fixed text-on-secondary-fixed rounded-lg border border-secondary-fixed-dim">
                        <p className="font-label-sm flex items-start gap-xs">
                            <Icon name="lightbulb" className="text-sm mt-0.5" />
                            <span>Remember to use the <strong>Flag for Review</strong> button on questions you want to revisit.</span>
                        </p>
                    </div>
                </aside>
            </main>

            <AppFooter compact />
        </div>
    );
}
