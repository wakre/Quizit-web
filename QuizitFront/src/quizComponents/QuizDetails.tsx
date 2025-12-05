import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { useAuth } from "../auth/AuthContext";
import "../css/QuizList.css";

import {Quiz} from "../types/Quiz";
import { Question } from "../types/Question";
import { Answer } from "../types/Answer";

//moved into types 
/*
interface Quiz {
  QuizId: number;
  Title: string;
  Description?: string;
  ImageUrl?: string;
  CategoryName: string;
  UserName: string;
  UserId: number;
  Questions: Question[];
}

interface Question {
  QuestionId: number;
  Text: string;
  Answers: Answer[];
}

interface Answer {
  AnswerId: number;
  Text: string;
  IsCorrect: boolean;
}
*/
const QuizDetails: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { user, token } = useAuth();
  const [quiz, setQuiz] = useState<Quiz | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchQuizDetails = async () => {
      try {
        const response = await fetch(`/api/quiz/${id}`);
        if (!response.ok)
          throw new Error(`Error fetching quiz: ${response.statusText}`);
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


  // moved the code section into their own pages for better structure
/*
  const handleDeleteQuiz = async () => {
    if (!window.confirm("Are you sure you want to delete this quiz?")) return;

    try {
      const response = await fetch(`/api/quiz/${id}`, {
        method: "DELETE",
        headers: { ...(token && { Authorization: `Bearer ${token}` }) },
      });
      if (!response.ok) throw new Error("Failed to delete quiz");

      alert("Quiz deleted successfully!");
      navigate("/QuizList");
    } catch (err: any) {
      setError(err.message || "Failed to delete quiz");
    }
  };

  const handleDeleteQuestion = async (questionId: number) => {
    if (!window.confirm("Are you sure you want to delete this question?"))
      return;

    try {
      const response = await fetch(`/api/question/${questionId}`, {
        method: "DELETE",
        headers: { ...(token && { Authorization: `Bearer ${token}` }) },
      });
      if (!response.ok) throw new Error("Failed to delete question");

      alert("Question deleted successfully!");
      window.location.reload();
    } catch (err: any) {
      setError(err.message || "Failed to delete question");
    }
  };
*/


  if (loading) return <p>Loading quiz details...</p>;
  if (error) return <p className="text-danger">{error}</p>;
  if (!quiz) return <p>Quiz not found.</p>;

  const isOwner = user && quiz.UserId === Number(user.userId);

  return (
    <div className="container mt-4">
      <button className="btn btn-secondary mb-3" onClick={() => navigate("/QuizList")}>
        ← Back to Quizzes
      </button>

      <div className="card">
        {quiz.ImageUrl && (
          <img
            src={quiz.ImageUrl}
            className="card-img-top"
            alt={`${quiz.Title}`}
          />
        )}

        <div className="card-body">
          <h2 className="card-title">{quiz.Title}</h2>
          {quiz.Description && <p className="card-text">{quiz.Description}</p>}
          <p className="card-text text-muted">
            Category: {quiz.CategoryName}
          </p>
          <p className="card-text text-muted">Created by: {quiz.UserName}</p>

          {/* Buttons visible to everyone, but only owner can use them */}
          <div className="mb-3">
            <button
              className="btn btn-warning me-2"
              onClick={() => {
                if (!isOwner)
                  return alert("Only the quiz owner can update this quiz.");
                navigate(`/UpdateQuiz/${quiz.QuizId}`);
              }}
            >
              Update Quiz
            </button>

            <button
              className="btn btn-danger"
              onClick={() => {
                if (!isOwner)
                  return alert("Only the quiz owner can delete this quiz.");
                navigate(`/DeleteQuiz/${quiz.QuizId}`)
              }}
              
            >
              Delete Quiz
            </button>

          </div>

          <h5>Questions ({quiz.Questions.length}):</h5>
          <ul className="list-group mb-3">
            {quiz.Questions.map((q) => (
              <li
                key={q.QuestionId}
                className="list-group-item d-flex justify-content-between align-items-center"
              >
                <div>
                  <strong>{q.Text}</strong> (Options: {q.Answers.length})
                </div>

                {/* Buttons visible to all, only allowed for owner */}
                <div>
                  <button
                    className="btn btn-sm btn-warning me-2"
                    onClick={() => {
                      if (!isOwner)
                        return alert("Only the quiz owner can edit questions.");
                      navigate(`/UpdateQuestion/${q.QuestionId}`);
                    }}
                  >
                    Edit
                  </button>

                  <button
                    className="btn btn-sm btn-danger"
                    onClick={() => {
                      if (!isOwner)
                        return alert("Only the quiz owner can delete questions.");
                      navigate(`/DeleteQuestion/${q.QuestionId}`)
                    }}
                  >
                    Delete
                  </button>

                </div>
              </li>
            ))}
          </ul>

          <button
            className="btn btn-primary"
            onClick={() => navigate(`/quiz/${quiz.QuizId}/play`)}
          >
            PLAY QUIZ
          </button>
        </div>
      </div>
    </div>
  );
};

export default QuizDetails;
