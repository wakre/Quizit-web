import { Question } from '../types/Question';

const API_BASE = '/api';

// Fetch a single question by ID
export const getQuestion = async (id: string): Promise<Question> => {
  const response = await fetch(`${API_BASE}/question/${id}`);
  if (!response.ok) {
    throw new Error(`Failed to fetch question: ${response.statusText}`);
  }
  return response.json();
};

// Create a new question (requires auth token)
export const createQuestion = async (
  data: { text: string; quizId: number; options: string[]; correctOptionIndex: number },
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
    throw new Error(`Failed to create question: ${response.statusText}`);
  }
};

// Update an existing question (requires auth token)
export const updateQuestion = async (
  id: string,
  data: { Text: string; Options: string[]; CorrectOptionIndex: number },
  token: string
): Promise<void> => {
  const response = await fetch(`${API_BASE}/question/${id}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(data),
  });
  if (!response.ok) {
    throw new Error(`Failed to update question: ${response.statusText}`);
  }
};

// Delete a question (requires auth token)
export const deleteQuestion = async (id: string, token: string): Promise<void> => {
  const response = await fetch(`${API_BASE}/question/${id}`, {
    method: 'DELETE',
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });
  if (!response.ok) {
    throw new Error(`Failed to delete question: ${response.statusText}`);
  }
};