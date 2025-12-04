import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { useAuth } from '../auth/AuthContext';

interface Question {
  QuestionId: number;
  Text: string;
  UserId: number;  // Match API
}

const DeleteQuestion: React.FC = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const { token, user } = useAuth();

  const [question, setQuestion] = useState<Question | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    const fetchQuestion = async () => {
      try {
        const res = await fetch(`/api/question/${id}`);
        if (!res.ok) throw new Error('Failed to load question');
        const data: Question = await res.json();

        if (user && Number(user.userId) !== data.UserId) {
          alert('You are not allowed to delete this question.');
          navigate(-1);
          return;
        }

        setQuestion(data);
      } catch (err: any) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };

    if (id) fetchQuestion();
  }, [id, user, navigate]);

  const deleteQuestion = async () => {
    try {
      const res = await fetch(`/api/question/${id}`, {
        method: 'DELETE',
        headers: {
          ...(token && { Authorization: `Bearer ${token}` }),
        },
      });
      if (!res.ok) throw new Error('Failed to delete question');
      alert('Question deleted successfully!');
      navigate(-1);
    } catch (err: any) {
      console.error(err);
      alert(err.message || 'Failed to delete question.');
    }
  };

  if (loading) return <p>Loading...</p>;
  if (error) return <p className="text-danger">{error}</p>;
  if (!question) return <p>Question not found.</p>;

  return (
    <div className="container mt-5">
      <h2>Delete Question</h2>
      <p>Are you sure you want to delete this question?</p>
      <p><strong>{question.Text}</strong></p>
      <button className="btn btn-danger me-2" onClick={deleteQuestion}>Delete</button>
      <button className="btn btn-secondary" onClick={() => navigate(-1)}>Cancel</button>
    </div>
  );
};

export default DeleteQuestion;