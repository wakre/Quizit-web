
import React from 'react';
import { useNavigate } from 'react-router-dom';
import 'bootstrap/dist/css/bootstrap.min.css';

const QuizList: React.FC = () => {
  const navigate = useNavigate();

  return (
    <div className="container mt-5">
      <h2>Available Quizzes</h2>
      {/* Quiz list will go here */}
    </div>
  );

}
export default QuizList;