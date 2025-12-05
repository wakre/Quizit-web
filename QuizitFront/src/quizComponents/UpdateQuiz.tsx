import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { useAuth } from "../auth/AuthContext";
import { Quiz } from "../types/Quiz";
import { Question } from "../types/Question";
import { Category } from "../types/Category";
import { getQuiz, updateQuiz, getCategories, addQuestion } from "./QuizServices";

/*
interface Category {
  CategoryId: number;  // Match API capitalization
  Name: string;
}

interface Question {
  QuestionId: number;
  Text: string;
  Answers: { AnswerId: number; Text: string; IsCorrect: boolean }[];
}

interface Quiz {
  QuizId: number;
  Title: string;
  Description?: string;
  ImageUrl?: string;
  CategoryId: number;
  UserId: number;  // Match API
  Questions: Question[];
}
*/
const UpdateQuiz: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { token, user } = useAuth();

  const [quiz, setQuiz] = useState<Quiz | null>(null);
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [imageUrl, setImageUrl] = useState("");
  const [categoryId, setCategoryId] = useState<number | "">("");
  const [categories, setCategories] = useState<Category[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  // For adding new questions
  const [showAddQuestion, setShowAddQuestion] = useState(false);
  const [newQuestionText, setNewQuestionText] = useState("");
  const [newOptions, setNewOptions] = useState<string[]>(["", ""]);
  const [newCorrectIndex, setNewCorrectIndex] = useState<number | null>(null);

  useEffect(() => {
    const loadQuiz = async () => {
      try {
        const data = await getQuiz(id!);
        
        /*
        const res = await fetch(`/api/quiz/${id}`);
        if (!res.ok) throw new Error("Failed to load quiz");

        const data: Quiz = await res.json();
        */
        if (user && data.UserId !== (user.userId)) {
          alert("You are not the owner of this quiz.");
          navigate(`/quiz/${id}`);
          return;
        }

        setQuiz(data);
        setTitle(data.Title);
        setDescription(data.Description || "");
        setImageUrl(data.ImageUrl || "");
        setCategoryId(data.CategoryId);
      } catch (err: any) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };

    const loadCategories = async () => {
      try {
        const data = await getCategories();
        setCategories(data);
      } catch (err: any) {
        setError(err.message);
      }
      /*
      const res = await fetch("/api/category");
      const data = await res.json();
      setCategories(data);
      */
    };

    if (id) {
      loadQuiz();
      loadCategories();
    }
  }, [id, user, navigate]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!token) return navigate("/login");

    try {
      await updateQuiz(id!, {
        Title: title,
        Description: description,
        ImageUrl: imageUrl.trim() === '' ? null : imageUrl.trim(),  // Always include, default to null if empty
        CategoryId: Number(categoryId),
      }, token);
      /*
      const body: any = {
        Title: title,
        Description: description,  // Always send (empty string is fine)
        CategoryId: categoryId,
      };
      if (imageUrl.trim()) {  // Only include ImageUrl if it's not empty/whitespace
        body.ImageUrl = imageUrl;
      }
      const res = await fetch(`/api/quiz/${id}`, {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify(body),  // Use the dynamic body
      });

      if (!res.ok) throw new Error("Update failed");
      */
      alert("Quiz updated successfully!");
      window.location.reload();  // Refresh to show changes
    } catch (err: any) {
      setError(err.message);
    }
  };

  const handleAddQuestion = async () => {
    if (!newQuestionText || newCorrectIndex === null || newOptions.some(opt => !opt)) {
      setError("Please fill all fields for the new question.");
      return;
    }

    try {
      await addQuestion({
        QuizId: id!,
        Text: newQuestionText,
        Options: newOptions,
        CorrectOptionIndex: newCorrectIndex,
      }, token!);
      /* 
      const res = await fetch("/api/question", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify({
          QuizId: id,
          Text: newQuestionText,
          Options: newOptions,
          CorrectOptionIndex: newCorrectIndex,
        }),
      });

      if (!res.ok) throw new Error("Failed to add question");
      */
      alert("Question added!");
      setShowAddQuestion(false);
      setNewQuestionText("");
      setNewOptions(["", ""]);
      setNewCorrectIndex(null);
      window.location.reload();  // Refresh to show new question
    } catch (err: any) {
      setError(err.message);
    }
  };

  

  if (loading) return <p>Loading quiz…</p>;
  if (!quiz) return <p>Quiz not found.</p>;

  return (
    <div className="container mt-4">
      <h2>Update Quiz</h2>

      <form onSubmit={handleSubmit}>
        {/* Existing fields */}
        <div className="mb-3">
          <label>Title</label>
          <input
            className="form-control"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            required
          />
        </div>

        <div className="mb-3">
          <label>Description</label>
          <textarea
            className="form-control"
            value={description}
            onChange={(e) => setDescription(e.target.value)}
          />
        </div>

        <div className="mb-3">
          <label>Image URL</label>
          <input
            className="form-control"
            value={imageUrl}
            onChange={(e) => setImageUrl(e.target.value)}
          />
        </div>

        <div className="mb-3">
          <label>Category</label>
          <select
            className="form-control"
            value={categoryId}
            onChange={(e) => setCategoryId(Number(e.target.value))}
            required
          >
            <option value="">Select</option>
            {categories.map((c) => (
              <option key={c.CategoryId} value={c.CategoryId}>
                {c.Name}
              </option>
            ))}
          </select>
        </div>

        {error && <p className="text-danger">{error}</p>}

        <button className="btn btn-primary">Save Quiz Changes</button>
        <button
          type="button"
          className="btn btn-secondary ms-2"
          onClick={() => navigate(`/quiz/${id}`)}
        >
          return
        </button>
      </form>

      {/* Question Management Section */}
      <h3 className="mt-4">Manage Questions</h3>
      <ul className="list-group mb-3">
        {quiz.Questions.map((q) => (
          <li key={q.QuestionId} className="list-group-item d-flex justify-content-between">
            <div>
              <strong>{q.Text}</strong> (Options: {q.Answers.length})
            </div>
            <div>
              <button
                className="btn btn-sm btn-warning me-2"
                onClick={() => navigate(`/UpdateQuestion/${q.QuestionId}`)}
              >
                Edit
              </button>
              
            </div>
          </li>
        ))}
      </ul>

      {/* Add New Question */}
      <button
        className="btn btn-success mb-3"
        onClick={() => setShowAddQuestion(!showAddQuestion)}
      >
        {showAddQuestion ? "Cancel Add Question" : "Add New Question"}
      </button>

      {showAddQuestion && (
        <div className="border p-3 mb-3">
          <h5>Add Question</h5>
          <input
            className="form-control mb-2"
            placeholder="Question Text"
            value={newQuestionText}
            onChange={(e) => setNewQuestionText(e.target.value)}
          />
          {newOptions.map((opt, i) => (
            <div key={i} className="d-flex mb-2">
              <input
                type="radio"
                name="newCorrect"
                checked={newCorrectIndex === i}
                onChange={() => setNewCorrectIndex(i)}
                className="me-2"
              />
              <input
                className="form-control"
                placeholder={`Option ${i + 1}`}
                value={opt}
                onChange={(e) => {
                  const arr = [...newOptions];
                  arr[i] = e.target.value;
                  setNewOptions(arr);
                }}
              />
              {newOptions.length > 2 && (
                <button
                  type="button"
                  className="btn btn-sm btn-danger ms-2"
                  onClick={() => setNewOptions(newOptions.filter((_, idx) => idx !== i))}
                >
                  X
                </button>
              )}
            </div>
          ))}
          {newOptions.length < 4 && (
            <button
              type="button"
              className="btn btn-secondary mb-2"
              onClick={() => setNewOptions([...newOptions, ""])}
            >
              Add Option
            </button>
          )}
          <button className="btn btn-primary" onClick={handleAddQuestion}>
            Add Question
          </button>
        </div>
      )}
    </div>
  );
};

export default UpdateQuiz;
