import React from 'react';
import { Nav, Navbar, Container, Button } from 'react-bootstrap';
import { useAuth } from '../auth/AuthContext';
import { useNavigate } from 'react-router-dom';

const NavMenu: React.FC = () => {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/');
  };

  return (
    <Navbar expand="lg" style={{ backgroundColor: "var(--deep-blue)" }} variant="dark">
      <Container>
        <Navbar.Brand href="/" style={{ color: "var(--cream-bg)" }}>
          QuizIt
        </Navbar.Brand>
        <Navbar.Toggle aria-controls="main-nav" />
        <Navbar.Collapse id="main-nav">
          <Nav className="ms-auto align-items-center">
            <Nav.Link href="/" style={{ color: "var(--cream-bg)" }}>
              Home
            </Nav.Link>
            <Nav.Link href="/QuizList" style={{ color: "var(--cream-bg)" }}>
              Quizzes
            </Nav.Link>
            {user && (
              <>
                <Nav.Link href="/CreateQuiz" style={{ color: "var(--cream-bg)" }}>
                  Create
                </Nav.Link>
                <Button variant="outline-light" className="ms-2" onClick={handleLogout}>
                  Logout
                </Button>
              </>
            )}
            {!user && (
              <>
                <Nav.Link href="/Login" style={{ color: "var(--cream-bg)" }}>
                  Login
                </Nav.Link>
                <Nav.Link href="/Register" style={{ color: "var(--cream-bg)" }}>
                  Register
                </Nav.Link>
              </>
            )}
          </Nav>
        </Navbar.Collapse>
      </Container>
    </Navbar>
  );
};

export default NavMenu;
