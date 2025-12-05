import React, { useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { createQuestion } from './QuestionServices';


const CreateQuestion: React.FC = () => {
  const { quizId } = useParams<{ quizId: string }>();
  const [text, setText] = useState('');
  const [options, setOptions] = useState(['', '']); // start with 2 options
  const [correctOption, setCorrectOption] = useState<number | ''>(''); // 1–4
  const [error, setError] = useState('');
  const navigate = useNavigate();

  const addOption = () => {
    if (options.length < 4) setOptions([...options, '']);
  };

  const removeOption = (index: number) => {
    if (options.length <= 2) return; // must have at least 2
    const newOptions = options.filter((_, i) => i !== index);

    // If removing the selected correct option, clear it
    if (correctOption === index + 1) setCorrectOption('');

    setOptions(newOptions);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    const token = localStorage.getItem('token');


    if (!token){
      setError('Uou must be logged in');
      return
    }
    if (correctOption === '') {
      setError('Please select the correct answer.');
      return;
    }

    const correctOptionIndex = Number(correctOption) - 1;


    try {
      await createQuestion(
        {
          text,
          quizId: Number(quizId),
          options,
          correctOptionIndex
        },
        token
      );
      /*  moved the api call to service
      const response = await fetch('/api/question', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          ...(token && { Authorization: `Bearer ${token}` }),
        },
        body: JSON.stringify({
          text,
          quizId: Number(quizId),
          options,
          correctOptionIndex,
        }),
      });

      if (!response.ok) throw new Error('Failed to add question');
      */
      alert('Question added! Add another or go back.');
      setText('');
      setOptions(['', '']); // reset to 2 options
      setCorrectOption('');
      setError('');
    } catch (err: any) {
      setError(err.message || 'Failed to add question');
    }
  };

  return (
    <div className="container mt-5">
      <h2>Add Question to Quiz</h2>
      <form onSubmit={handleSubmit}>
        {/* QUESTION TEXT */}
        <div className="mb-3">
          <label>Question Text</label>
          <input
            type="text"
            className="form-control"
            value={text}
            onChange={(e) => setText(e.target.value)}
            required
          />
        </div>

        {/* OPTIONS */}
        {options.map((opt, index) => (
          <div key={index} className="mb-3 d-flex align-items-center">
            <div className="flex-grow-1">
              <label>Option {index + 1}</label>
              <input
                type="text"
                className="form-control"
                value={opt}
                onChange={(e) => {
                  const newOpts = [...options];
                  newOpts[index] = e.target.value;
                  setOptions(newOpts);
                }}
                required
              />
            </div>

            {/* Remove button */}
            {options.length > 2 && (
              <button
                type="button"
                className="btn btn-danger ms-2"
                onClick={() => removeOption(index)}
              >
                ✕
              </button>
            )}
          </div>
        ))}

        {/* ADD OPTION BUTTON */}
        {options.length < 4 && (
          <button
            type="button"
            className="btn btn-secondary mb-3"
            onClick={addOption}
          >
            + Add Option
          </button>
        )}

        {/* CORRECT ANSWER RADIO BUTTONS */}
        <div className="mb-3">
          <label>Correct Answer</label>
          <div className="d-flex gap-4 mt-2">
            {options.map((_, index) => (
              <label key={index} className="d-flex align-items-center">
                <input
                  type="radio"
                  name="correct"
                  value={index + 1}
                  checked={correctOption === index + 1}
                  onChange={() => setCorrectOption(index + 1)}
                  className="form-check-input me-2"
                  required
                />
                {index + 1}
              </label>
            ))}
          </div>
        </div>

        {/* ERROR */}
        {error && <p className="text-danger">{error}</p>}

        {/* BUTTONS */}
        <button type="submit" className="btn btn-primary">
          Add Question
        </button>

        <button
          type="button"
          className="btn btn-secondary ms-2"
          onClick={() => navigate('/')}
        >
          Finish
        </button>
      </form>
    </div>
  );
};

export default CreateQuestion;
