import { useParams, useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import { useAuth } from '../auth/AuthContext';
import { Quiz } from "../types/Quiz";
import { getQuiz,deleteQuiz } from "./QuizServices";

/*
interface Quiz {
  QuizId: number;
  Title: string;
  UserId: number;  // Match API
}
*/
const DeleteQuiz = () => {
  const { id } = useParams<{id: string}>();
  const navigate = useNavigate();
  const { token, user } = useAuth();

  const [quiz, setQuiz] = useState<Quiz | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const loadQuiz = async () => {
      try {
        const data = await getQuiz(id!);
        if (user && data.UserId !== (user.userId)) {
          alert("You are not the owner of this quiz.");
          navigate(`/quiz/${id}`);
          return;
        }
        setQuiz(data);
      } catch (err: any) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };
    if (id) loadQuiz();
  }, [id, user, navigate]);

  

  const handleDelete = async () => {
    if (!token) {
      navigate('/Login');
      return;
    }
    try {
      await deleteQuiz(id!, token);
      /*
      await fetch(`/api/quiz/${id}`, {
        method: "DELETE",
        headers: { Authorization: `Bearer ${token}` }
      });
      */
      alert("Quiz deleted successfully!");
      navigate("/QuizList");
    } catch (err) {
      console.error(err);
      alert("Failed to delete quiz.");
    }
  };

  if (loading) return <div>Loading...</div>;
  if (error) return <div>{error}</div>;
  if (!quiz) return <div>Quiz not found.</div>;

  return (
    <div className="delete-card">
      <h1>Delete Quiz</h1>
      <p>
        Are you sure you want to delete <strong>{quiz.Title}</strong>?
      </p>
      <button className="btn btn-danger me-2" onClick={handleDelete}>
        Delete
      </button>
      <button className="btn btn-secondary" onClick={() => navigate(-1)}>
        Cancel
      </button>
    </div>
  );
};

export default DeleteQuiz;