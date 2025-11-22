using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using api.DAL;
using api.DTOs;
using api.Models;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionRepository _repo;
        private readonly ILogger<QuestionController> _logger;

        public QuestionController(IQuestionRepository repo, ILogger<QuestionController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        // Create a question, (only owner) 
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] QuestionCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            
            var quiz = await _repo.GetById(dto.QuizId); 
            if (quiz == null)
                return NotFound(new { message = "Quiz not found." });

            var userId = int.Parse(User.FindFirst("userId")!.Value);
            if (quiz.UserId != userId)
                return Unauthorized(new { message = "You don't have access to add questions to this quiz." });

            if (quiz.Questions.Count >= 10)
                return BadRequest(new { message = "Quiz cannot have more than 10 questions." });

            var question = new Question
            {
                Text = dto.Text,
                QuizId = dto.QuizId,
                Answers = dto.Options.Select((opt, index) => new Answer
                {
                    Text = opt,
                    IsCorrect = index == dto.CorrectOptionIndex
                }).ToList()
            };

            var created = await _repo.Create(question);
            if (created == null)
                return StatusCode(500, "Error creating question.");

            return Ok(new { message = "Question added successfully!", questionId = created.QuestionId });
        }

        // Update a question, (only owner) 
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] QuestionCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var question = await _repo.GetWithAnswers(id);
            if (question == null)
                return NotFound(new { message = "Question not found." });

            var userId = int.Parse(User.FindFirst("userId")!.Value);
            if (question.Quiz.UserId != userId)
                return Unauthorized(new { message = "You don't have access to update this question." });

            question.Text = dto.Text;
            // Update answers (simplified)
            var updated = await _repo.Update(question);
            if (updated == null)
                return StatusCode(500, "Error updating question.");

            return Ok(new { message = "Question updated successfully!" });
        }

        // Delete a question, (only owner) 
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var question = await _repo.GetWithAnswers(id);
            if (question == null)
                return NotFound(new { message = "Question not found." });

            var userId = int.Parse(User.FindFirst("userId")!.Value);
            if (question.Quiz.UserId != userId)
                return Unauthorized(new { message = "You don't have access to delete this question." });

            var success = await _repo.Delete(id);
            if (!success)
                return StatusCode(500, "Error deleting question.");

            return Ok(new { message = "Question deleted successfully!" });
        }
    }
}