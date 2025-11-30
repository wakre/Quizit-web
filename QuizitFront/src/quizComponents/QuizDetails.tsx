import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";  // For URL params and navigation
import "../css/QuizList.css";  // Reuse styles

// Interface matches QuizDto from backend
interface Quiz {
  QuizId: number;
  Title: string;
  Description?: string;
  ImageUrl?: string;
  CategoryName: string;
  UserName: string;
  Questions: Question[];  // Full details from GetById
}

interface Question {
  QuestionId: number;
  Text: string;
  Answers: Answer[];  // For display (answers hidden in details)
}

interface Answer {
  AnswerId: number;
  Text: string;
  IsCorrect: boolean;  // Hidden in UI to avoid spoilers
}

const QuizDetails: React.FC = () => {
  const { id } = useParams<{ id: string }>();  // Get QuizId from URL
  const navigate = useNavigate();
  const [quiz, setQuiz] = useState<Quiz | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchQuizDetails = async () => {
      try {
        const response = await fetch(`/api/quiz/${id}`);
        if (!response.ok) throw new Error(`Error fetching quiz: ${response.statusText}`);
        const data: Quiz = await response.json();
        setQuiz(data);
      } catch (err: any) {
        setError(err.message || "Unknown error");
      } finally {
        setLoading(false);
      }
    };

    if (id) fetchQuizDetails();
  }, [id]);

  if (loading) return <p>Loading quiz details...</p>;
  if (error) return <p className="text-danger">{error}</p>;
  if (!quiz) return <p>Quiz not found.</p>;

  return (
    <div className="container mt-4">
      <button className="btn btn-secondary mb-3" onClick={() => navigate(-1)}>← Back to Quizzes</button>  {/* Back button */}
      <div className="card">
        {quiz.ImageUrl && (
          <img src={quiz.ImageUrl} className="card-img-top" alt={`${quiz.Title}`} />
        )}
        <div className="card-body">
          <h2 className="card-title">{quiz.Title}</h2>
          {quiz.Description && <p className="card-text">{quiz.Description}</p>}
          <p className="card-text text-muted">Category: {quiz.CategoryName}</p>
          <p className="card-text text-muted">Created by: {quiz.UserName}</p>
          <h5>Questions ({quiz.Questions.length}):</h5>
          <ul className="list-group mb-3">
            {quiz.Questions.map((q) => (
              <li key={q.QuestionId} className="list-group-item">
                <strong>{q.Text}</strong> (Options: {q.Answers.length})  {/* Hide answers to avoid spoilers */}
              </li>
            ))}
          </ul>
          <button
            className="btn btn-details"
            onClick={() => alert(`Start taking quiz ID: ${quiz.QuizId}`)}  // Placeholder: Link to quiz-taking logic
          >
            Take Quiz
          </button>
        </div>
      </div>
    </div>
  );
};

export default QuizDetails;