import React from 'react';
import { Nav,Navbar } from 'react-bootstrap';

const NavMenu = () => {
  return (
    <Navbar expand="lg">
      <Navbar.Brand href="/">QuizIt</Navbar.Brand>
      <Nav className="me-auto">
        <Nav.Link href="/">Home</Nav.Link>
      </Nav>
    </Navbar>
  );
};

export default NavMenu;