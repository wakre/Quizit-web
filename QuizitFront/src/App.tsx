// src/App.tsx
import React, { useEffect } from 'react';
import { BrowserRouter as Router, Route, Routes, useLocation } from 'react-router-dom';
import Container from 'react-bootstrap/Container';
import Homepage from './home/HomePage';
import QuizList from './quizComponents/QuizList';
import QuizDetails from './quizComponents/QuizDetails';
import CreateQuiz from './quizComponents/CreateQuiz';
import TakeQuiz from './quizComponents/TakeQuiz';
import QuizResults from './quizComponents/QuizResults';
import NavMenu from './shared/NavMenu';
import Footer from './shared/Footer';
import './App.css'
import Login from './auth/LogIn';
import Register from './auth/Register';
import CreateQuestion from './questionsComponents/CreateQuestion';
import { AuthProvider } from './auth/AuthContext';
import { ProtectedRoute } from './auth/ProtectedRoute';
import UpdateQuiz from './quizComponents/UpdateQuiz';
import UpdateQuestion from './questionsComponents/UpdateQuestion';
import DeleteQuiz from './quizComponents/DeleteQuiz';
import DeleteQuestion from './questionsComponents/DeleteQuestion';

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
        {/*  public routes  */}
        <Route path="/" element={<Homepage />} />
        <Route path="/QuizList" element={<Container className="my-4"><QuizList /></Container>} />
        <Route path="/TakeQuiz/:id" element ={<Container className='my-4'><TakeQuiz /></Container>}/>
        <Route path="/quiz/:id" element={<Container className="my-4"><QuizDetails /></Container>} />
        <Route path="/Login" element={<Container className="my-4"><Login /></Container>} />
        <Route path= "/Register" element= {<Container className="my-4"><Register /></Container>}/>
        <Route path="/quiz/:id/play" element={<Container className="my-4"><TakeQuiz /></Container>} />
        <Route path="/quiz/:id/results" element={<Container className="my-4"><QuizResults /></Container>} />

        
        {/* protected routes */}
        <Route path="/CreateQuiz" element={<ProtectedRoute><Container className="my-4"><CreateQuiz /></Container></ProtectedRoute>} />
        <Route path="/CreateQuestion/:quizId" element={<ProtectedRoute><Container className="my-4"><CreateQuestion /></Container></ProtectedRoute>} />
        <Route path="/UpdateQuiz/:id" element={<ProtectedRoute><Container className="my-4"><UpdateQuiz /></Container></ProtectedRoute>} />
        <Route path="/DeleteQuiz/:id" element={<ProtectedRoute><Container className="my-4"><DeleteQuiz /></Container></ProtectedRoute>} />
        <Route path="/UpdateQuestion/:id" element={<ProtectedRoute><Container className="my-4"><UpdateQuestion /></Container></ProtectedRoute>} />
        <Route path="/DeleteQuestion/:id" element={<ProtectedRoute><Container className="my-4"><DeleteQuestion /></Container></ProtectedRoute>} />
      </Routes>
      <Footer />
    </>
  );
};
const App: React.FC = () => {
  return (
    <AuthProvider>
      <Router>
        <AppContent />
      </Router>
    </AuthProvider>
  );
};
export default App;
