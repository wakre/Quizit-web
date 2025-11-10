// src/App.tsx
import React from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Homepage from './home/HomePage';
import QuizList from './quizComponents/QuizList';
import CreateQuiz from './quizComponents/CreateQuiz';

const App: React.FC = () => {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Homepage />} />
        <Route path="/QuizList" element={<QuizList />} />
        <Route path="/create-quiz" element={<CreateQuiz />} />
      </Routes>
    </BrowserRouter>
  );
};

export default App;
