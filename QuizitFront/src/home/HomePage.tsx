import React from 'react';
import { Link } from 'react-router-dom';
import '../css/HomePage.css';
import 'bootstrap/dist/css/bootstrap.min.css';

const Homepage: React.FC = () => {
  return (
    <div className="homepage quiz-main">
      <h1 className='display4'>Welcome to QUIZIT</h1>
      <p className="lead">Choose a mode to get started</p>

      <div className="row justify-content-center" style={{ marginTop: '50px' }}>
        <div className="col-sm-10">
          <div className="row">
            <div className="col-6">
              <Link
                to="/QuizList"
                className="btn btn-success btn-lg w-100 homepage-btn"
                style={{
                  height: '100px',
                  display: 'flex',
                  alignItems: 'center',
                  justifyContent: 'center',
                  opacity: '90%'
                }}
              >
                View Available Quizzes
              </Link>
            </div>
            <div className="col-6">
              <Link
                to="/CreateQuiz"
                className="btn btn-primary btn-lg w-100 homepage-btn"
                style={{
                  height: '100px',
                  display: 'flex',
                  alignItems: 'center',
                  justifyContent: 'center',
                  opacity: '90%'
                }}
              >
                Create Your Own Quiz
              </Link>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Homepage;
