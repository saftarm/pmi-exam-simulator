import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import './Exam.css';

function Exam() {
    const { id } = useParams();         // examId from URL /exam/:id/start
    const navigate = useNavigate();

    const [question, setQuestion] = useState(null);  // single question
    const [currentIndex, setCurrentIndex] = useState(0);   // which question we're on
    const [totalQuestions, setTotalQuestions] = useState(0);
    const [selectedAnswer, setSelectedAnswer] = useState(null);
    const [answers, setAnswers] = useState([]);    // accumulated answers
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [finished, setFinished] = useState(false);

    // Fetch one question by examId + index
    useEffect(() => {
        const fetchQuestion = async () => {
            setLoading(true);
            setError(null);
            setSelectedAnswer(null);

            try {
                const res = await fetch(`/api/Exam/${id}/question?index=${currentIndex}`);
                if (!res.ok) {
                    const body = await res.text();
                    throw new Error(`${res.status} ${res.statusText}: ${body}`);
                }
                const data = await res.json();
                console.log('Question data:', data);

                setQuestion(data.question);
                setTotalQuestions(data.totalQuestions);
            } catch (err) {
                console.error('Failed to fetch question:', err);
                setError(err.message);
            } finally {
                setLoading(false);
            }
        };

        fetchQuestion();
    }, [id, currentIndex]); // re-runs every time index changes

    const isLast = currentIndex === totalQuestions - 1;

    const handleSelect = (option) => {
        setSelectedAnswer(option);
    };

    const handleNext = () => {
        if (!selectedAnswer) return;

        // Save the answer
        const updatedAnswers = [...answers, {
            questionId: question.id,
            questionText: question.title,
            selectedOptionId: selectedAnswer.id,
            selectedOptionText: selectedAnswer.text,
        }];
        setAnswers(updatedAnswers);

        if (isLast) {
            setFinished(true);
        } else {
            setCurrentIndex(prev => prev + 1); // triggers useEffect to fetch next
        }
    };

    // ── Finished screen ───────────────────────────────
    if (finished) return (
        <div className="page">
            <div className="ed-orb" />
            <div className="header">
                <h1 className="header-title">📋 CAPM Certification</h1>
                <span className="header-badge">Completed</span>
            </div>
            <div className="finished-screen">
                <h2 className="finished-title">Exam Complete! 🎉</h2>
                <p className="finished-sub">
                    You answered {answers.length} of {totalQuestions} questions.
                </p>
                <div className="finished-actions">
                    <button
                        className="next-button active"
                        onClick={() => navigate('/exams')}
                    >
                        Back to Exams
                    </button>
                </div>
            </div>
        </div>
    );

    // ── Loading ───────────────────────────────────────
    if (loading) return (
        <div className="page">
            <div className="ed-orb" />
            <div className="header">
                <h1 className="header-title">📋 CAPM Certification</h1>
                <span className="header-badge">Loading...</span>
            </div>
            <div className="loading-msg">Loading question {currentIndex + 1}...</div>
        </div>
    );

    // ── Error ─────────────────────────────────────────
    if (error) return (
        <div className="page">
            <div className="ed-orb" />
            <div className="header">
                <h1 className="header-title">📋 CAPM Certification</h1>
                <span className="header-badge">Error</span>
            </div>
            <div className="error-msg">Error: {error}</div>
        </div>
    );

    if (!question) return null;

    const progress = Math.round(((currentIndex + 1) / totalQuestions) * 100);

    return (
        <div className="page">
            <div className="ed-orb" />

            {/* ── Header ───────────────────────────────── */}
            <div className="header">
                <h1 className="header-title">📋 CAPM Certification</h1>
                <span className="header-badge">
                    Question {currentIndex + 1} / {totalQuestions}
                </span>
            </div>

            <div className="main">

                {/* ── Left panel — question ─────────────── */}
                <div className="panel">
                    <div>
                        <span className="panel-label">Question {currentIndex + 1}</span>
                        <h2 className="question-text">{question.title}</h2>
                    </div>
                    <div className="progress-bar-wrapper">
                        <div className="progress-bar-labels">
                            <span className="progress-bar-label">Progress</span>
                            <span className="progress-bar-percent">{progress}%</span>
                        </div>
                        <div className="progress-bar-track">
                            <div
                                className="progress-bar-fill"
                                style={{ width: `${progress}%` }}
                            />
                        </div>
                    </div>
                </div>

                {/* ── Right panel — answers ─────────────── */}
                <div className="panel">
                    <div>
                        <span className="panel-sublabel">Select your answer</span>
                        <ul className="options-list">
                            {question.answerOptionsDtos?.map((option, i) => {
                                const isSelected = selectedAnswer?.id === option.id;
                                return (
                                    <li key={option.id ?? i}>
                                        <button
                                            onClick={() => handleSelect(option)}
                                            className={`option-button ${isSelected ? 'selected' : 'default'}`}
                                        >
                                            <span className={`option-letter ${isSelected ? 'selected' : 'default'}`}>
                                                {String.fromCharCode(65 + i)}
                                            </span>
                                            {option.text}
                                        </button>
                                    </li>
                                );
                            })}
                        </ul>
                    </div>
                    <button
                        onClick={handleNext}
                        disabled={!selectedAnswer}
                        className={`next-button ${selectedAnswer ? 'active' : 'inactive'}`}
                    >
                        {isLast ? 'Finish Exam ✓' : 'Next Question →'}
                    </button>
                </div>

            </div>
        </div>
    );
}

export default Exam;