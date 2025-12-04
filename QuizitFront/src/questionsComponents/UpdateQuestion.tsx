import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { useAuth } from "../auth/AuthContext";

interface Answer {
  AnswerId: number;
  Text: string;
  IsCorrect: boolean;
}

interface Question {
  QuestionId: number;
  Text: string;
  UserId: number;  // Added for ownership check
  Answers: Answer[];
}

const UpdateQuestion: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { token, user } = useAuth();

  const [question, setQuestion] = useState<Question | null>(null);
  const [text, setText] = useState("");
  const [options, setOptions] = useState<string[]>(["", ""]);
  const [correctIndex, setCorrectIndex] = useState<number | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    const loadQuestion = async () => {
      try {
        const res = await fetch(`/api/question/${id}`);
        if (!res.ok) throw new Error("Failed to load question");
        const q: Question = await res.json();

        if (user && q.UserId !== Number(user.userId)) {
          alert("You are not the owner of this question.");
          navigate(-1);
          return;
        }

        setQuestion(q);
        setText(q.Text);
        const texts = q.Answers.map((a) => a.Text);
        setOptions(texts);
        const correct = q.Answers.findIndex((a) => a.IsCorrect);
        setCorrectIndex(correct >= 0 ? correct : null);
      } catch (err: any) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };

    if (id) loadQuestion();
  }, [id, user, navigate]);

  const addOption = () => {
    if (options.length >= 4) return;
    setOptions([...options, ""]);
  };

  const removeOption = (index: number) => {
    if (options.length <= 2) return;
    setOptions(options.filter((_, i) => i !== index));
    if (correctIndex === index) setCorrectIndex(null);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (correctIndex === null) {
      setError("Please select a correct answer.");
      return;
    }

    try {
      const res = await fetch(`/api/question/${id}`, {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify({
          Text: text,  // Match API capitalization
          Options: options,
          CorrectOptionIndex: correctIndex,
        }),
      });

      if (!res.ok) throw new Error("Failed to update question");

      alert("Question updated successfully!");
      navigate(-1);
    } catch (err: any) {
      setError(err.message);
    }
  };

  if (loading) return <p>Loading question…</p>;
  if (!question) return <p>Question not found.</p>;

  return (
    <div className="container mt-4">
      <h2>Update Question</h2>

      <form onSubmit={handleSubmit}>
        <div className="mb-3">
          <label>Question</label>
          <input
            className="form-control"
            value={text}
            onChange={(e) => setText(e.target.value)}
            required
          />
        </div>

        <h5>Options (2–4)</h5>
        {options.map((opt, i) => (
          <div key={i} className="d-flex align-items-center mb-2">
            <input
              type="radio"
              name="correct"
              checked={correctIndex === i}
              onChange={() => setCorrectIndex(i)}
              className="me-2"
            />
            <input
              className="form-control"
              value={opt}
              onChange={(e) => {
                const arr = [...options];
                arr[i] = e.target.value;
                setOptions(arr);
              }}
              required
            />
            {options.length > 2 && (
              <button
                type="button"
                className="btn btn-sm btn-danger ms-2"
                onClick={() => removeOption(i)}
              >
                X
              </button>
            )}
          </div>
        ))}

        {options.length < 4 && (
          <button type="button" className="btn btn-secondary mb-3" onClick={addOption}>
            Add Option
          </button>
        )}

        {error && <p className="text-danger">{error}</p>}

        <button className="btn btn-primary">Save</button>
        <button type="button" className="btn btn-secondary ms-2" onClick={() => navigate(-1)}>
          Cancel
        </button>
      </form>
    </div>
  );
};

export default UpdateQuestion;