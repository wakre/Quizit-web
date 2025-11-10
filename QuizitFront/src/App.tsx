// src/App.tsx
import React from 'react';
import { BrowserRouter as Router, Route, Routes, Navigate } from 'react-router-dom';
import Container from 'react-bootstrap/Container'
import Homepage from './home/HomePage';
import QuizList from './quizComponents/QuizList';
import CreateQuiz from './quizComponents/CreateQuiz';
import NavMenu from './shared/NavMenu';

const App: React.FC = () => {
  return (
    <Container>
      <NavMenu />
      <Router>
        <Routes>
          <Route path="/" element={<Homepage />} />
          <Route path="/QuizList" element={<QuizList />} />
          <Route path="/createQuiz" element={<CreateQuiz />} />
        </Routes>
      </Router>
    </Container>
  );
};

export default App;
