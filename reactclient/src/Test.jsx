//import { useState , useEffect} from 'react'
//import reactLogo from './assets/react.svg'
//import viteLogo from '/vite.svg'
import './App.css'
import { useState, useEffect } from 'react';

export default function Test() {

    const [questions, setQuestions] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [currentIndex, setCurrentIndex] = useState(0);
    const [selectedAnswer, setSelectedAnswer] = useState(null);
    const [answers, setAnswers] = useState([]);

    useEffect( () => {
        const fetchQuestions = async () => {
            try {
                const res = await fetch("/api/Question/GetAllQuestions");
                if(!res.ok) throw new Error("Failed to fetch questions");
                const data = await res.json();
                console.log("Questions: ", data);
                setQuestions(data);

            }
            catch(err) {
                setError(err.message);
            }
            finally {
                setLoading(false);
            }
        };

        fetchQuestions();
    }, []);

    const currentQuestion = questions[currentIndex];
    const isLast = currentIndex === questions.length - 1;
    const handleSelect = (option) => {
        setSelectedAnswer(option);
    }
    const handleNext = () => {
        if(!selectedAnswer) return;
    }
    
    setAnswers([...answers, {
        question: currentQuestion.Text,
        selected: selectedAnswer.Text,
        isCorrect: selectedAnswer.isCorrect,

    }]);
    setSelectedAnswer(null);    
    if(!isLast) {
        setCurrentIndex(currentIndex + 1);
    }
    else {
        console.log("Exam finished", answers);
    }

    if(loading) return <p> Loading questions </p>;
    if(error) return <p> Error: {error}</p>;
    if(!questions.length) return <p>No questions found</p>;

    return (

        <div>
      <p>{currentIndex + 1} / {questions.length}</p>

      <h2>{currentQuestion.text}</h2>

      <ul>
        {currentQuestion.answerOptions.map((option, i) => (
          <li key={i}>
            <button
              onClick={() => handleSelect(option)}
              style={{
                fontWeight: selectedAnswer === option ? "bold" : "normal",
                outline: selectedAnswer === option ? "2px solid blue" : "none",
              }}
            >
              {option.text}
            </button>
          </li>
        ))}
      </ul>

      <button onClick={handleNext} disabled={!selectedAnswer}>
        {isLast ? "Finish" : "Next"}
      </button>
    </div>



    );
        

}


