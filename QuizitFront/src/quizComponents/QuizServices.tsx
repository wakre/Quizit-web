import { Quiz } from '../types/Quiz'; 
import { Category } from '../types/Category';
import { Question } from '../types/Question';

const API_BASE = '/api';  // Base URL for the API

// Fetch all quizzes
export const getQuizzes = async (): Promise<Quiz[]> => {
  const response = await fetch(`${API_BASE}/quiz`);
  if (!response.ok) {
    throw new Error(`Failed to fetch quizzes: ${response.statusText}`);
  }
  return response.json();
};

// Fetch a single quiz by ID
export const getQuiz = async (id: string): Promise<Quiz> => {
  const response = await fetch(`${API_BASE}/quiz/${id}`);
  if (!response.ok) {
    throw new Error(`Failed to fetch quiz: ${response.statusText}`);
  }
  return response.json();
};

// Create a new quiz (requires auth token)
export const createQuiz = async (
  data: { title: string; description: string; imageUrl: string | null; categoryId: number },
  token: string
): Promise<{ quizId: number }> => {
  const response = await fetch(`${API_BASE}/quiz`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(data),
  });
  if (!response.ok) {
    throw new Error(`Failed to create quiz: ${response.statusText}`);
  }
  return response.json();
};

// Update an existing quiz (requires auth token)
export const updateQuiz = async (
  id: string,
  data: { Title: string; Description: string; ImageUrl: string | null; CategoryId: number },
  token: string
): Promise<void> => {
  const response = await fetch(`${API_BASE}/quiz/${id}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(data),
  });
  if (!response.ok) {
    throw new Error(`Failed to update quiz: ${response.statusText}`);
  }
};

// Delete a quiz (requires auth token)
export const deleteQuiz = async (id: string, token: string): Promise<void> => {
  const response = await fetch(`${API_BASE}/quiz/${id}`, {
    method: 'DELETE',
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });
  if (!response.ok) {
    throw new Error(`Failed to delete quiz: ${response.statusText}`);
  }
};

// Fetch all categories
export const getCategories = async (): Promise<Category[]> => {
  const response = await fetch(`${API_BASE}/category`);
  if (!response.ok) {
    throw new Error(`Failed to fetch categories: ${response.statusText}`);
  }
  return response.json();
};

// Add a new question to a quiz (requires auth token)
export const addQuestion = async (
  data: { QuizId: string; Text: string; Options: string[]; CorrectOptionIndex: number },
  token: string
): Promise<void> => {
  const response = await fetch(`${API_BASE}/question`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(data),
  });
  if (!response.ok) {
    throw new Error(`Failed to add question: ${response.statusText}`);
  }
};