
// src/home/Homepage.tsx
import React from 'react';
import { useNavigate } from 'react-router-dom';
import 'bootstrap/dist/css/bootstrap.min.css';

const Homepage: React.FC = () => {
  const navigate = useNavigate();

  return (
    <div className="text-center" style={{ marginTop: '100px' }}>
      <h1 className="display-4">Welcome to QUIZIT</h1>
      <p className="lead">Choose a mode to get started</p>

      <div className="row justify-content-center" style={{ marginTop: '50px' }}>
        <div className="col-sm-6">
          <div className="row">
            <div className="col-6">
              <button
                className="btn btn-success btn-lg w-100"
                style={{ height: '100px', paddingTop: '30px' }}
                onClick={() => navigate('/QuizList.tsx')}
              >
                View Available Quizzes
              </button>
            </div>
            <div className="col-6">
              <button
                className="btn btn-primary btn-lg w-100"
                style={{ height: '100px', paddingTop: '30px' }}
                onClick={() => navigate('/create-quiz')}
              >
                Create Your Own Quiz
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Homepage;
