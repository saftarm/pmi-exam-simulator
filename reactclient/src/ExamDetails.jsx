import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import './ExamDetails.css';

function ExamDetails() {
    const { id } = useParams();
    const navigate = useNavigate();

    const [exam, setExam] = useState(null);
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
                console.log('Exam data:', data);
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

    return (
        <div className="ed-page">
            <div className="ed-orb" />

            {/* ── Header ───────────────────────────────── */}
            <div className="ed-header">
                <button className="ed-back-btn" onClick={() => navigate('/exams')}>
                    ← Back
                </button>
                <span className="ed-header-badge">
                    {questions.length} Questions
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
                        <span className="ed-stat-num">{questions.length}</span>
                        <span className="ed-stat-lbl">Questions</span>
                    </div>
                </div>
            </div>

            {/* ── Questions list ────────────────────────── */}
            {questions.length > 0 ? (
                <div className="ed-questions">
                    <h2 className="ed-section-title">
                        Questions ({questions.length})
                    </h2>
                    <div className="ed-question-list">
                        {questions.map((q, i) => (
                            <div className="ed-question-card" key={q.id ?? i}>
                                <div className="ed-question-num">Q{i + 1}</div>
                                <div className="ed-question-body">
                                    <p className="ed-question-text">{q.title}</p>

                                    {q.answerOptionsDtos && q.answerOptionsDtos.length > 0 && (
                                        <div className="ed-options">
                                            {q.answerOptionsDtos.map((opt, oi) => (
                                                <div className="ed-option" key={opt.id ?? oi}>
                                                    <span className="ed-option-letter">
                                                        {String.fromCharCode(65 + oi)}
                                                    </span>
                                                    <span className="ed-option-text">{opt.text}</span>
                                                </div>
                                            ))}
                                        </div>
                                    )}
                                </div>
                            </div>
                        ))}
                    </div>
                </div>
            ) : (
                <div className="ed-state-msg">No questions found for this exam.</div>
            )}

            {/* ── Start button ─────────────────────────── */}
            <div className="ed-cta">
                <button
                    className="ed-start-btn"
                    onClick={() => navigate(`/exam/${id}/start`)}
                >
                    Start Exam →
                </button>
            </div>
        </div>
    );
}

export default ExamDetails;