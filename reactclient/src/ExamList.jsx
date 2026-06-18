import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from './context/AuthContext';
import { getPublishedExams, startSession } from './services/examService';
import './ExamList.css';

function ExamList() {
    const navigate = useNavigate();
    const { user, logout } = useAuth();

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
            navigate(`/exams/${examId}/session/${session.sessionId}`);
        } catch (err) {
            console.error('Failed to start session:', err);
            alert('Could not start exam session. Please try again.');
        } finally {
            setStartingId(null);
        }
    };

    const handleLogout = () => {
        logout();
        navigate('/');
    };

    if (loading) return (
        <div className="el-page">
            <div className="el-orb" />
            <div className="el-header">
                <h1 className="header-title">📋 PMI Exam Simulator</h1>
                <span className="header-badge">Loading...</span>
            </div>
            <div className="el-state-msg">Loading exams...</div>
        </div>
    );

    if (error) return (
        <div className="el-page">
            <div className="el-orb" />
            <div className="el-header">
                <h1 className="header-title">📋 PMI Exam Simulator</h1>
                <span className="header-badge">Error</span>
            </div>
            <div className="el-state-msg el-state-error">Error: {error}</div>
        </div>
    );

    const totalQuestions = exams.reduce((a, e) => a + (e.numberOfQuestions ?? 0), 0);

    return (
        <div className="el-page">
            <div className="el-orb" />

            <div className="el-header">
                <h1 className="header-title">📋 PMI Exam Simulator</h1>
                <div className="el-header-actions">
                    <span className="header-badge">
                        Welcome, {user?.userName ?? 'User'}
                    </span>
                    <button className="el-logout-btn" onClick={handleLogout}>
                        Log Out
                    </button>
                </div>
            </div>

            <div className="el-hero">
                <div className="el-hero-left">
                    <span className="el-eyebrow">Practice Library</span>
                    <h2 className="el-title">Choose your exam</h2>
                    <p className="el-subtitle">
                        Select an exam to practice with realistic PMI-style questions and track your progress.
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
                    </div>
                </div>
            </div>

            {!exams.length ? (
                <div className="el-state-msg">No published exams available yet.</div>
            ) : (
                <div className="el-grid">
                    {exams.map((exam, i) => (
                        <div
                            className="el-card"
                            key={exam.id}
                            style={{ animationDelay: `${i * 0.06}s` }}
                        >
                            <div className="el-card-top">
                                <div className="el-card-badge">📋</div>
                            </div>

                            <div className="el-card-body">
                                <h3 className="el-card-title">{exam.title}</h3>
                                <p className="el-card-desc">{exam.context || 'Practice exam'}</p>
                            </div>

                            <div className="el-card-footer">
                                <div className="el-card-meta">
                                    <span className="el-meta-item">🗒 {exam.numberOfQuestions ?? '—'} questions</span>
                                    <span className="el-meta-item">⏱ {exam.durationInMinutes ?? '—'} min</span>
                                </div>
                                <button
                                    className="el-card-btn"
                                    disabled={startingId === exam.id}
                                    onClick={() => handleStart(exam.id)}
                                >
                                    {startingId === exam.id ? 'Starting...' : 'Start →'}
                                </button>
                            </div>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
}

export default ExamList;
