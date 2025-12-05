import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../auth/AuthContext';
import { Category } from '../types/Category';
import { createQuiz,getCategories } from './QuizServices';


/*
interface Category {
  CategoryId: number;
  Name: string;*/


const CreateQuiz: React.FC = () => {
  const [title, setTitle] = useState('');
  const [description, setDescription] = useState('');
  const [imageUrl, setImageUrl] = useState(''); // optional
  const [categoryId, setCategoryId] = useState<number | ''>('');
  const [categories, setCategories] = useState<Category[]>([]);
  const [error, setError] = useState('');
  const navigate = useNavigate();
  const { token } = useAuth();

  useEffect(() => {
    const fetchCategories = async () => {
      try {
        const data= await getCategories();
        /*
        removed fetch into a service api call:
        const response = await fetch('/api/category');
        if (!response.ok) throw new Error('Failed to load categories');
        const data = await response.json();
        */
        setCategories(data);
      } catch (err: any) {
        setError(err.message);
      }
    };

    fetchCategories();
  }, []);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!token) {
      navigate('/Login');
      return;
    }

    // Convert imageUrl to null if user leaves it empty (cleaner for backend)
    const imageToSend = imageUrl.trim() === '' ? null : imageUrl.trim();


    //creating the quiz
    try {
      
      const data = await createQuiz({
        title: title.trim(),
        description: description.trim(),
        imageUrl: imageToSend,
        categoryId: Number(categoryId),
      }, token);

      /* moved into services 
      const response = await fetch('/api/quiz', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify({
          title: title.trim(),
          description: description.trim(),
          imageUrl: imageToSend,
          categoryId: Number(categoryId),
        }),
      });

      if (!response.ok) throw new Error('Failed to create quiz');

      const data = await response.json();
      */

      navigate(`/CreateQuestion/${data.quizId}`);

    } catch (err: any) {
      setError(err.message || 'Failed to create quiz');
    }
  };

  return (
    <div className="container mt-5">
      <h2>Create Quiz</h2>

      <form onSubmit={handleSubmit}>

        {/* TITLE */}
        <div className="mb-3">
          <label>Title</label>
          <input 
            type="text"
            className="form-control"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            required
          />
        </div>

        {/* DESCRIPTION */}
        <div className="mb-3">
          <label>Description</label>
          <textarea 
            className="form-control"
            value={description}
            onChange={(e) => setDescription(e.target.value)}
          />
        </div>

        {/* OPTIONAL IMAGE URL */}
        <div className="mb-3">
          <label>Image URL (optional)</label>
          <input
            type="url"
            className="form-control"
            value={imageUrl}
            onChange={(e) => setImageUrl(e.target.value)}
            placeholder="https://example.com/image.png"
          />
        </div>

        {/* CATEGORY */}
        <div className="mb-3">
          <label>Category</label>
          <select
            className="form-control"
            value={categoryId}
            onChange={(e) => setCategoryId(Number(e.target.value))}
            required
          >
            <option value="">Select Category</option>
            {categories.map((cat) => (
              <option key={cat.CategoryId} value={cat.CategoryId}>
                {cat.Name}
              </option>
            ))}
          </select>
        </div>

        {error && <p className="text-danger">{error}</p>}

        <button type="submit" className="btn btn-primary">
          Create Quiz
        </button>
      </form>
    </div>
  );
};

export default CreateQuiz;
