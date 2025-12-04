import React from 'react';
import { useLocation, useNavigate } from 'react-router-dom';

const QuizResults: React.FC = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const { score, total } = location.state || { score: 0, total: 0 };

  return (
    <div className="container mt-5 text-center">
      <h2>Quiz Results</h2>
      <div className="card">
        <div className="card-body">
          <h3>Your Score: {score} / {total}</h3>
          <p>{score === total ? 'Perfect!' : score > total / 2 ? 'Good job!' : 'Keep practicing!'}</p>
          <button className="btn btn-primary me-2" onClick={() => navigate('/QuizList')}>
            Take Another Quiz
          </button>
          <button className="btn btn-secondary" onClick={() => navigate('/')}>
            Home
          </button>
        </div>
      </div>
    </div>
  );
};

export default QuizResults;