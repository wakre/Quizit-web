// src/components/QuizList.tsx
import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";  // Assuming you're using React Router for navigation
import "../css/QuizList.css";

// Updated interface to match QuizDto from the backend
interface Quiz {
  QuizId: number;          // Matches API: QuizId
  Title: string;           // Matches API: Title
  Description?: string;    // Matches API: Description
  ImageUrl?: string;       // Optional: For displaying quiz images
  CategoryName: string;    // New: From QuizDto, for showing category
  UserName: string;        // New: From QuizDto, for showing creator (optional in UI)
  // Questions: QuestionDto[];  // Not needed for list view; omit to keep it lightweight
}

const QuizList: React.FC = () => {
  const [quizzes, setQuizzes] = useState<Quiz[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();  // For navigation to quiz details

  useEffect(() => {
    const fetchQuizzes = async () => {
      try {
        const response = await fetch("/api/quiz");  // Matches your backend route
        if (!response.ok) throw new Error(`Error fetching quizzes: ${response.statusText}`);
        const data: Quiz[] = await response.json();  // Now matches QuizDto structure
        setQuizzes(data);
      } catch (err: any) {
        setError(err.message || "Unknown error");
      } finally {
        setLoading(false);
      }
    };

    fetchQuizzes();
  }, []);

  if (loading) return <p>Loading quizzes...</p>;
  if (error) return <p className="text-danger">{error}</p>;
return (
    <div className="container mt-4">
      <h2 className="mb-4">Available Quizzes</h2>
      <div className="row">
        {quizzes.map((quiz) => (
          <div className="col-sm-6 col-md-4 col-lg-3 mb-4" key={quiz.QuizId}>
            <div className="card quiz-card h-100">
              {quiz.ImageUrl && (
                <img src={quiz.ImageUrl} className="card-img-top" alt={`${quiz.Title} thumbnail`} />
              )}
              <div className="card-body d-flex flex-column">
                <h5 className="card-title">{quiz.Title}</h5>
                <p className="card-text text-muted">Category: {quiz.CategoryName}</p>  {/* Only category shown */}
                <button
                  className="btn btn-details mt-auto"
                  onClick={() => navigate(`/quiz/${quiz.QuizId}`)}  // Navigate to details page
                >
                  View Details
                </button>
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};
export default QuizList;

/*
import React, { useEffect, useState } from "react";
import "../css/QuizList.css";

interface Quiz {
  id: number;
  title: string;
  description?: string;
}

const QuizList: React.FC = () => {
  const [quizzes, setQuizzes] = useState<Quiz[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchQuizzes = async () => {
      try {
        const response = await fetch("/api/quiz");
        if (!response.ok) throw new Error(`Error fetching quizzes: ${response.statusText}`);
        const data: Quiz[] = await response.json();
        setQuizzes(data);
      } catch (err: any) {
        setError(err.message || "Unknown error");
      } finally {
        setLoading(false);
      }
    };

    fetchQuizzes();
  }, []);

  if (loading) return <p>Loading quizzes...</p>;
  if (error) return <p className="text-danger">{error}</p>;

  return (
    <div className="container mt-4">
      <h2 className="mb-4">Available Quizzes</h2>
      <div className="row">
        {quizzes.map((quiz) => (
          <div className="col-sm-6 col-md-4 col-lg-3 mb-4" key={quiz.id}>
            <div className="card quiz-card h-100">
              <div className="card-body d-flex flex-column">
                <h5 className="card-title">{quiz.title}</h5>
                {quiz.description && <p className="card-text">{quiz.description}</p>}
                <button
                  className="btn btn-details mt-auto"
                  onClick={() => alert(`Show details for quiz ID: ${quiz.id}`)}
                >
                  Details
                </button>
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default QuizList;

*/