import { useState, useEffect } from 'react';
import './App.css';

function App() {
  const [questions, setQuestions] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [currentIndex, setCurrentIndex] = useState(0);
  const [selectedAnswer, setSelectedAnswer] = useState(null);
  const [answers, setAnswers] = useState([]);

  const [finished, setFinished] = useState([]);

  useEffect(() => {
    const fetchQuestions = async () => {
      try {
        const res = await fetch("/api/Question");
        if (!res.ok) throw new Error("Failed to fetch questions");
        const data = await res.json();
        setQuestions(data);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };
    fetchQuestions();
  }, []);

  const currentQuestion = questions[currentIndex];
  const isLast = currentIndex === questions.length - 1;

  const handleSelect = (option) => {
    setSelectedAnswer(option);
  };

  const handleNext = () => {
    if (!selectedAnswer) return;

    const updatedAnswers = setAnswers([...answers, {
      question: currentQuestion.text,
      selected: selectedAnswer.text,
      isCorrect: selectedAnswer.isCorrect,
    }]);

    setAnswers(updatedAnswers);
    setSelectedAnswer(null);

    if (!isLast) {
      setCurrentIndex(currentIndex + 1);
    } else {
      setFinished(true);
    }
  };

  if (loading) return <p>Loading questions...</p>;
  if (error) return <p>Error: {error}</p>;
  if (!questions.length) return <p>No questions found.</p>;

  if(finished) { 

  return (
    <div className="page">

      <div className="header">
        <h1 className="header-title">📋 CAPM Certification</h1>
        <span className="header-badge">
          Question {currentIndex + 1} / {questions.length}
        </span>
      </div>

      <div className="main">

        {/* Left Panel */}
        <div className="panel">
          <div>
            <span className="panel-label">Question {currentIndex + 1}</span>
            <h2 className="question-text">{currentQuestion.text}</h2>
          </div>
          <div className="progress-bar-wrapper">
            <div className="progress-bar-labels">
              <span className="progress-bar-label">Progress</span>
              <span className="progress-bar-percent">
                {Math.round(((currentIndex + 1) / questions.length) * 100)}%
              </span>
            </div>
            <div className="progress-bar-track">
              <div
                className="progress-bar-fill"
                style={{ width: `${((currentIndex + 1) / questions.length) * 100}%` }}
              />
            </div>
          </div>
        </div>

        {/* Right Panel */}
        <div className="panel">
          <div>
            <span className="panel-sublabel">Select your answer</span>
            <ul className="options-list">
              {currentQuestion.answerOptions.map((option, i) => {
                const isSelected = selectedAnswer === option;
                return (
                  <li key={i}>
                    <button
                      onClick={() => handleSelect(option)}
                      className={`option-button ${isSelected ? "selected" : "default"}`}
                    >
                      <span className={`option-letter ${isSelected ? "selected" : "default"}`}>
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
            className={`next-button ${selectedAnswer ? "active" : "inactive"}`}
          >
            {isLast ? "Finish Exam ✓" : "Next Question →"}


          </button>
        </div>

      </div>
    </div>
  );}

}



export default App;