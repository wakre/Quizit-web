import React, { createContext, useContext, useState, useEffect, ReactNode } from 'react';

interface User{
  userId: number;
  userName: string;
  email: string;
}
interface AuthContextType {
  user: User | null; // e.g., { userId, userName, email }
  token: string | null;
  login: (token: string, user: any) => void;
  logout: () => void;
  isAuthenticated: boolean;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
  const [token, setToken] = useState<string | null>(localStorage.getItem('token'));
  const [user, setUser] = useState<any>(JSON.parse(localStorage.getItem('user') || 'null'));

  const login = (newToken: string, newUser: any) => {
  const mappedUser = {
    userId: newUser.UserId,  // Map from API's UserId to lowercase userId
    userName: newUser.UserName,
    email: newUser.Email
  };
  
  setToken(newToken);
  setUser(mappedUser);  // Use the mapped object
  localStorage.setItem('token', newToken);
  localStorage.setItem('user', JSON.stringify(mappedUser));  // Store the mapped object
};

  const logout = () => {
    setToken(null);
    setUser(null);
    localStorage.removeItem('token');
    localStorage.removeItem('user');
  };

  const isAuthenticated = !!token;

  return (
    <AuthContext.Provider value={{ user, token, login, logout, isAuthenticated }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) throw new Error('useAuth must be used within AuthProvider');
  return context;
};