// src/App.tsx
import React from 'react';
import { BrowserRouter as Router, Route, Routes, Navigate } from 'react-router-dom';
import Container from 'react-bootstrap/Container'
import Homepage from './home/HomePage';
import QuizList from './quizComponents/QuizList';
import CreateQuiz from './quizComponents/CreateQuiz';
import NavMenu from './shared/NavMenu';
import Footer from './shared/Footer';
import './App.css'

const App: React.FC = () => {
  return (
    <Router>
      <NavMenu />
      <Container className="my-4"> 
        <Routes>
          <Route path="/" element={<Homepage />} />
          <Route path="/QuizList" element={<QuizList />} />
          <Route path="/createQuiz" element={<CreateQuiz />} />
        </Routes>
      </Container>
      <Footer />
    </Router>
    
  );
};

export default App;
