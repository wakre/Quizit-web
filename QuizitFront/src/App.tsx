// src/App.tsx
import React, { useEffect } from 'react';
import { BrowserRouter as Router, Route, Routes, useLocation } from 'react-router-dom';
import Container from 'react-bootstrap/Container';
import Homepage from './home/HomePage';
import QuizList from './quizComponents/QuizList';
import CreateQuiz from './quizComponents/CreateQuiz';
import NavMenu from './shared/NavMenu';
import Footer from './shared/Footer';
import './App.css'

const AppContent: React.FC =() => {
    const location = useLocation();
  useEffect(() => {
    if (location.pathname === '/') {
      document.body.classList.add('homepage-bg');
    } else {
      document.body.classList.remove('homepage-bg');
    }
  }, [location.pathname]);
  return (
    <>
      <NavMenu />
      <Routes>
        <Route path="/" element={<Homepage />} />
        <Route path="/QuizList" element={<Container className="my-4"><QuizList /></Container>} />
        <Route path="/CreateQuiz" element={<Container className="my-4"><CreateQuiz /></Container>} />
      </Routes>
      <Footer />
    </>
  );
};
const App: React.FC = () => {
  return (
    <Router>
      <AppContent />
    </Router>
  );
};
export default App;
/*
const App: React.FC = () => {
  return (
    <Router>
         <NavMenu />
         <Routes>
           <Route path="/" element={<Homepage />} />  {/* No Container here for full-screen background *//*}
           <Route path="/QuizList" element={<Container className="my-4"><QuizList /></Container>} />
           <Route path="/CreateQuiz" element={<Container className="my-4"><CreateQuiz /></Container>} />
         </Routes>
         <Footer />
       </Router>
    
  );
};
export default App;*/
