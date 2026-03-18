import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import './ExamList.css';

const DIFFICULTY_COLOR = {
    Easy: 'el-badge-easy',
    Medium: 'el-badge-medium',
    Hard: 'el-badge-hard',
};

function ExamList() {
    const navigate = useNavigate();

    const [exams, setExams] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchExams = async () => {
            try {
                const res = await fetch('/api/Summary');
                if (!res.ok) {
                    const body = await res.text();
                    throw new Error(`${res.status} ${res.statusText}: ${body}`);
                }
                const data = await res.json();
                console.log('Exams:', data);
                setExams(data);
            } catch (err) {
                console.error('Failed to fetch exams:', err);
                setError(err.message);
            } finally {
                setLoading(false);
            }
        };

        fetchExams();
    }, []);

    const handleStart = async (examId) => {
        try {
            const res = await fetch(`/api/ExamAttempt/${examId}`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ examId }),
            });
            if (!res.ok) throw new Error('Failed to create attempt');
            const data = await res.json();
            navigate(`/examDetails/${examId}?attemptId=${data}`);
        } catch (err) {
            console.error(err);
        }
    };

    if (loading) return (
        <div className="el-page">
            <div className="el-orb" />
            <div className="el-header">
                <h1 className="header-title">📋 CAPM Certification</h1>
                <span className="header-badge">Loading...</span>
            </div>
            <div className="el-state-msg">Loading exams...</div>
        </div>
    );

    if (error) return (
        <div className="el-page">
            <div className="el-orb" />
            <div className="el-header">
                <h1 className="header-title">📋 CAPM Certification</h1>
                <span className="header-badge">Error</span>
            </div>
            <div className="el-state-msg el-state-error">Error: {error}</div>
        </div>
    );

    if (!exams.length) return (
        <div className="el-page">
            <div className="el-orb" />
            <div className="el-header">
                <h1 className="header-title">📋 CAPM Certification</h1>
                <span className="header-badge">0 Exams</span>
            </div>
            <div className="el-state-msg">No exams found.</div>
        </div>
    );

    // FIX: was e.questions, now correctly uses e.numberOfQuestions
    const totalQuestions = exams.reduce((a, e) => a + (e.numberOfQuestions ?? 0), 0);

    return (
        <div className="el-page">
            <div className="el-orb" />

            {/* ── Header ───────────────────────────────── */}
            <div className="el-header">
                <h1 className="header-title">📋 CAPM Certification</h1>
                <span className="header-badge">{exams.length} Exams</span>
            </div>

            {/* ── Hero row ─────────────────────────────── */}
            <div className="el-hero">
                <div className="el-hero-left">
                    <span className="el-eyebrow">Practice Library</span>
                    <h2 className="el-title">Choose your exam</h2>
                    <p className="el-subtitle">
                        Select a topic to practice or take the full exam to simulate real conditions.
                    </p>
                </div>
                <div className="el-hero-right">
                    <div className="el-summary">
                        <div className="el-summary-item">
                            <span className="el-summary-num">{exams.length}</span>
                            <span className="el-summary-lbl">Exams</span>
                        </div>
                        <div className="el-summary-divider" />
                        <div className="el-summary-item">
                            <span className="el-summary-num">{totalQuestions}</span>
                            <span className="el-summary-lbl">Questions</span>
                        </div>
                        <div className="el-summary-divider" />
                        <div className="el-summary-item">
                            <span className="el-summary-num">5</span>
                            <span className="el-summary-lbl">Domains</span>
                        </div>
                    </div>
                </div>
            </div>

            {/* ── Exam grid ────────────────────────────── */}
            <div className="el-grid">
                {exams.map((exam, i) => (
                    <div
                        className="el-card"
                        key={exam.id}
                        style={{ animationDelay: `${i * 0.06}s` }}
                    >
                        {/* Card top */}
                        <div className="el-card-top">
                            <div className="el-card-badge">{exam.badge ?? '📋'}</div>
                            <span className={`el-difficulty ${DIFFICULTY_COLOR[exam.difficulty] ?? 'el-badge-medium'}`}>
                                {exam.difficulty ?? 'Medium'}
                            </span>
                        </div>

                        {/* Card body */}
                        <div className="el-card-body">
                            <span className="el-card-domain">{exam.domain ?? 'General'}</span>
                            <h3 className="el-card-title">{exam.title}</h3>
                            <p className="el-card-desc">{exam.description}</p>
                        </div>

                        {/* Card footer */}
                        <div className="el-card-footer">
                            <div className="el-card-meta">
                                <span className="el-meta-item">🗒 {exam.numberOfQuestions ?? '—'} questions</span>
                                <span className="el-meta-item">⏱ {exam.durationInMinutes ?? '—'} min</span>
                            </div>
                            <button
                                className="el-card-btn"
                                onClick={() => handleStart(exam.id)}
                            >
                                Start →
                            </button>
                        </div>
                    </div>
                ))}
            </div>

        </div>
    );
}

export default ExamList;