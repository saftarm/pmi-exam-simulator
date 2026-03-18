import { useState, useEffect } from 'react';
import { useParams, useNavigate, useSearchParams } from 'react-router-dom';
import './ExamDetails.css';

function ExamDetails() {
    const { id } = useParams();
    const navigate = useNavigate();
    const [searchParams] = useSearchParams();
    const attemptId = Number(searchParams.get('attemptId'));


    const [exam, setExam] = useState(null);
    const [currentIndex, setCurrentIndex] = useState(0);
    const [selectedAnswer, setSelectedAnswer] = useState(null);
    const [saving, setSaving] = useState(false);
    const [saveError, setSaveError] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchExam = async () => {
            try {
                const res = await fetch(`/api/Exam/Details/${id}`);
                if (!res.ok) {
                    const body = await res.text();
                    throw new Error(`${res.status} ${res.statusText}: ${body}`);
                }
                const data = await res.json();
                setExam(data);
            } catch (err) {
                console.error('Failed to fetch exam:', err);
                setError(err.message);
            } finally {
                setLoading(false);
            }
        };

        fetchExam();
    }, [id]);

    if (loading) return (
        <div className="ed-page">
            <div className="ed-orb" />
            <div className="ed-state-msg">Loading exam...</div>
        </div>
    );

    if (error) return (
        <div className="ed-page">
            <div className="ed-orb" />
            <div className="ed-state-msg ed-state-error">Error: {error}</div>
        </div>
    );

    if (!exam) return null;

    const questions = exam.questionDtos ?? [];
    const total = questions.length;
    const current = questions[currentIndex];
    const isFirst = currentIndex === 0;
    const isLast = currentIndex === total - 1;
    const progress = total > 0 ? Math.round(((currentIndex + 1) / total) * 100) : 0;

    const handleSelect = (option) => {
        setSaveError(null);
        setSelectedAnswer(option);
    };

    const handleNext = async () => {
        if (!selectedAnswer) return;

        setSaving(true);
        setSaveError(null);

        try {
            const res = await fetch(`/api/ExamAttempt/save`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    examAttemptId: Number(attemptId),
                    questionId: current.id,
                    selectedOptionId: selectedAnswer.id,
                }),
            });

            if (!res.ok) {
                const body = await res.text();
                throw new Error(`Failed to save answer: ${res.status} ${body}`);
            }

            if (isLast) {

                const finishRes = await fetch(`/api/ExamAttempt/finish/${attemptId}`, {
                    method: 'POST'
                });

                if (!finishRes.ok) {
                    const body = await finishRes.text();
                    throw new Error(`Failed to finish exam: ${finishRes.status} ${body}`);
                }

                navigate(`/exam/${id}/finish?attemptId=${attemptId}`);

            } else {
                setCurrentIndex(i => i + 1);
                setSelectedAnswer(null);
            }

        } catch (err) {
            console.error(err);
            setSaveError(err.message);
        } finally {
            setSaving(false);
        }
    };

    return (
        <div className="ed-page">
            <div className="ed-orb" />

            {/* ── Header ───────────────────────────────── */}
            <div className="ed-header">
                <button className="ed-back-btn" onClick={() => navigate('/exams')}>
                    ← Back
                </button>
                <span className="ed-header-badge">
                    Question {currentIndex + 1} / {total}
                </span>
            </div>

            {/* ── Hero ─────────────────────────────────── */}
            <div className="ed-hero">
                <div className="ed-hero-left">
                    <span className="ed-eyebrow">Practice Exam</span>
                    <h1 className="ed-title">Exam #{id}</h1>
                </div>
                <div className="ed-stats">
                    <div className="ed-stat-item">
                        <span className="ed-stat-num">{total}</span>
                        <span className="ed-stat-lbl">Questions</span>
                    </div>
                    <div className="ed-stat-divider" />
                    <div className="ed-stat-item">
                        <span className="ed-stat-num">{progress}%</span>
                        <span className="ed-stat-lbl">Progress</span>
                    </div>
                </div>
            </div>

            {/* ── Progress bar ─────────────────────────── */}
            <div className="ed-progress-wrapper">
                <div className="ed-progress-track">
                    <div className="ed-progress-fill" style={{ width: `${progress}%` }} />
                </div>
            </div>

            {/* ── Question card ─────────────────────────── */}
            {current && (
                <div className="ed-question-card">
                    <div className="ed-question-num">Q{currentIndex + 1}</div>
                    <div className="ed-question-body">
                        <p className="ed-question-text">{current.title}</p>

                        {current.answerOptionsDtos && current.answerOptionsDtos.length > 0 && (
                            <div className="ed-options">
                                {current.answerOptionsDtos.map((opt, oi) => {
                                    const isSelected = selectedAnswer?.id === opt.id;
                                    return (
                                        <div
                                            key={opt.id ?? oi}
                                            className={`ed-option ${isSelected ? 'ed-option--selected' : ''}`}
                                            onClick={() => handleSelect(opt)}
                                        >
                                            <span className={`ed-option-letter ${isSelected ? 'ed-option-letter--selected' : ''}`}>
                                                {String.fromCharCode(65 + oi)}
                                            </span>
                                            <span className="ed-option-text">{opt.text}</span>
                                        </div>
                                    );
                                })}
                            </div>
                        )}
                    </div>
                </div>
            )}

            {/* ── Save error ───────────────────────────── */}
            {saveError && (
                <div className="ed-state-msg ed-state-error" style={{ marginTop: 16 }}>
                    {saveError}
                </div>
            )}

            {/* ── Navigation ───────────────────────────── */}
            <div className="ed-nav">
                <button
                    className="ed-nav-btn"
                    onClick={() => { setCurrentIndex(i => i - 1); setSelectedAnswer(null); }}
                    disabled={isFirst || saving}
                >
                    ← Previous
                </button>

                <button
                    className={`ed-nav-btn ed-nav-btn--next ${!selectedAnswer || saving ? 'ed-nav-btn--disabled' : ''}`}
                    onClick={handleNext}
                    disabled={!selectedAnswer || saving}
                >
                    {saving
                        ? 'Saving...'
                        : isLast
                            ? 'Finish Exam ✓'
                            : 'Next →'}
                </button>
            </div>
        </div>
    );
}

export default ExamDetails;