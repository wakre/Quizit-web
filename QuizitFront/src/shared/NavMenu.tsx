import React from 'react';
import { Nav,Navbar,Container } from 'react-bootstrap';

const NavMenu = () => {
  return(
  <Navbar expand="lg" style={{ backgroundColor: "var(--deep-blue)" }} variant="dark">
      <Container>
        <Navbar.Brand href="/" style={{ color: "var(--cream-bg)" }}>
          QuizIt
        </Navbar.Brand>
        <Navbar.Toggle aria-controls="main-nav" />
        <Navbar.Collapse id="main-nav">
          <Nav className="ms-auto">
            <Nav.Link href="/" style={{ color: "var(--cream-bg)" }}>
              Home
            </Nav.Link>
            <Nav.Link href="/QuizList" style={{ color: "var(--cream-bg)" }}>
              Quizzes
            </Nav.Link>
            <Nav.Link href="/createQuiz" style={{ color: "var(--cream-bg)" }}>
              Create
            </Nav.Link>
          </Nav>
        </Navbar.Collapse>
      </Container>
    </Navbar>
  );
};


export default NavMenu;