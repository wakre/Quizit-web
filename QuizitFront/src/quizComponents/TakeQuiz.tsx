import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { Quiz } from '../types/Quiz';
import { Question } from '../types/Question';

/*
interface Quiz {
  QuizId: number;
  Title: string;
  Questions: Question[];
}

interface Question {
  QuestionId: number;
  Text: string;
  Answers: { AnswerId: number; Text: string; IsCorrect: boolean }[];
}
*/

const TakeQuiz: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [quiz, setQuiz] = useState<Quiz | null>(null);
  const [currentQuestionIndex, setCurrentQuestionIndex] = useState(0);
  const [selectedAnswers, setSelectedAnswers] = useState<number[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');


  // fetching the quiz questions to answer
  useEffect(() => {
    const fetchQuiz = async () => {
      try {
        const response = await fetch(`/api/quiz/${id}`);
        if (!response.ok) throw new Error('Failed to load quiz');
        const data: Quiz = await response.json();
        setQuiz(data);
        setSelectedAnswers(new Array(data.Questions.length).fill(-1)); // Initialize with -1 (no selection)
      } catch (err: any) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };
    if (id) fetchQuiz();
  }, [id]);

  const handleAnswerSelect = (answerIndex: number) => {
    const newAnswers = [...selectedAnswers];
    newAnswers[currentQuestionIndex] = answerIndex;
    setSelectedAnswers(newAnswers);
  };

  const handleNext = () => {
    if (currentQuestionIndex < (quiz?.Questions.length || 0) - 1) {
      setCurrentQuestionIndex(currentQuestionIndex + 1);
    }
  };


  //error handling for unanswered questions
  const handleSubmit = async () => {
    if (selectedAnswers.includes(-1)) {
      alert('Please answer all questions.');
      return;
    }
    try {
      const response = await fetch(`/api/quiz/${id}/submit`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(selectedAnswers),
      });
      if (!response.ok) throw new Error('Failed to submit quiz');
      const result = await response.json();
      navigate(`/quiz/${id}/results`, { state: { score: result.score, total: result.total } });
    } catch (err: any) {
      setError(err.message || 'Failed to submit quiz');
    }
  };

  if (loading) return <p>Loading quiz...</p>;
  if (error) return <p className="text-danger">{error}</p>;
  if (!quiz || quiz.Questions.length === 0) return <p>No questions available.</p>;

  const currentQuestion = quiz.Questions[currentQuestionIndex];
  const isLastQuestion = currentQuestionIndex === quiz.Questions.length - 1;

  return (
    <div className="container mt-5">
      <h2>{quiz.Title}</h2>
      <div className="card">
        <div className="card-body">
          <h5>Question {currentQuestionIndex + 1} of {quiz.Questions.length}</h5>
          <p className="card-text">{currentQuestion.Text}</p>
          <div className="mb-3">
            {currentQuestion.Answers.map((answer, index) => (
              <div key={answer.AnswerId} className="form-check">
                <input
                  className="form-check-input"
                  type="radio"
                  name="answer"
                  id={`answer-${index}`}
                  checked={selectedAnswers[currentQuestionIndex] === index}
                  onChange={() => handleAnswerSelect(index)}
                />
                <label className="form-check-label" htmlFor={`answer-${index}`}>
                  {answer.Text}
                </label>
              </div>
            ))}
          </div>
          <div className="d-flex justify-content-between">
            <button
              className="btn btn-secondary"
              onClick={() => setCurrentQuestionIndex(currentQuestionIndex - 1)}
              disabled={currentQuestionIndex === 0}
            >
              Previous
            </button>
            {!isLastQuestion ? (
              <button
                className="btn btn-primary"
                onClick={handleNext}
                disabled={selectedAnswers[currentQuestionIndex] === -1}
              >
                Next
              </button>
            ) : (
              <button
                className="btn btn-success"
                onClick={handleSubmit}
                disabled={selectedAnswers[currentQuestionIndex] === -1}
              >
                Submit Quiz
              </button>
            )}
          </div>
        </div>
      </div>
    </div>
  );
};

export default TakeQuiz;