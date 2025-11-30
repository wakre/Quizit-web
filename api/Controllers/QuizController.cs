using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using api.DAL;
using api.DTOs;
using api.Models;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuizController : ControllerBase
    {
        private readonly IQuizRepository _repo;
        private readonly ILogger<QuizController> _logger;

        public QuizController(IQuizRepository repo, ILogger<QuizController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        // Get all quizzes (public for guest mode)
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var quizzes = await _repo.GetAll();
            if (quizzes == null)
                return StatusCode(500, "Error retrieving quizzes.");
            
            //Quizdtos to populate quizzes
            var quizDtos = quizzes.Select(q => new QuizDto
            {
                QuizId = q.QuizId,
                Title = q.Title,
                Description = q.Description,
                ImageUrl = q.ImageUrl,
                CategoryId = q.CategoryId,
                CategoryName = q.Category?.Name ?? "Unknown",  // Flatten to string (no cycle)
                UserId = q.UserId,
                UserName = q.User?.UserName ?? "Unknown",     // Flatten to string (no cycle)
                Questions = new List<QuestionDto>()           // Empty for list view; populate if needed
            }).ToList();

            return Ok(quizDtos);
        }

        // Get quiz by ID (public for taking)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var quiz = await _repo.GetQuizWithQuestions(id);
            if (quiz == null)
                return NotFound(new { message = "Quiz not found." });
            //some quizdto logic to get the data from dto not ef
            var quizDto = new QuizDto
            {
                QuizId = quiz.QuizId,
                Title = quiz.Title,
                Description = quiz.Description,
                ImageUrl = quiz.ImageUrl,
                CategoryId = quiz.CategoryId,
                CategoryName = quiz.Category?.Name ?? "Unknown",
                UserId = quiz.UserId,
                UserName = quiz.User?.UserName ?? "Unknown",
                Questions = quiz.Questions?.Select(qst => new QuestionDto
                {
                    QuestionId = qst.QuestionId,
                    Text = qst.Text,
                    Answers = qst.Answers?.Select(ans => new AnswerDto
                    {
                        AnswerId = ans.AnswerId,
                        Text = ans.Text,
                        IsCorrect = ans.IsCorrect  // Include for quiz-taking; hide in list view if needed
                    }).ToList() ?? new List<AnswerDto>()
                }).ToList() ?? new List<QuestionDto>()
            };

            return Ok(quizDto);
        }

        // Create quiz (auth required)
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] QuizCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = int.Parse(User.FindFirst("userId")!.Value);

            // Manuell mapping fra DTO til entity
            var quiz = new Quiz
            {
                Title = dto.Title,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl,
                CategoryId = dto.CategoryId,
                UserId = userId,
                DateCreated = DateTime.UtcNow
            };

            var created = await _repo.Create(quiz);
            if (created == null)
                return StatusCode(500, "Error creating quiz.");

            // Kommentar: Returnerer quizId til frontend
            return Ok(new { message = "Quiz created successfully!", quizId = created.QuizId });
        }

        // Update quiz (auth required)
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] QuizUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var quiz = await _repo.GetById(id);
            if (quiz == null)
                return NotFound(new { message = "Quiz not found." });

            var userId = int.Parse(User.FindFirst("userId")!.Value);
            if (quiz.UserId != userId)
                return Unauthorized(new { message = "You don't have rights to update this quiz." });

            // Manuell mapping fra DTO til eksisterende entity
            quiz.Title = dto.Title;
            quiz.Description = dto.Description;
            quiz.ImageUrl = dto.ImageUrl;
            quiz.CategoryId = dto.CategoryId;

            var updated = await _repo.Update(quiz);
            if (updated == null)
                return StatusCode(500, "Error updating quiz.");

            // Kommentar: Returmelding for frontend
            return Ok(new { message = "Quiz updated successfully!" });
        }

        // Delete quiz (auth required)
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var quiz = await _repo.GetById(id);
            if (quiz == null)
                return NotFound(new { message = "Quiz not found." });

            var userId = int.Parse(User.FindFirst("userId")!.Value);
            if (quiz.UserId != userId)
                return Unauthorized(new { message = "You don't have rights to delete this quiz." });

            var success = await _repo.Delete(id);
            if (!success)
                return StatusCode(500, "Error deleting quiz.");

            // Kommentar: Bekreftelse på sletting
            return Ok(new { message = "Quiz deleted successfully!" });
        }

        // Submit quiz answers and calculate score
        [HttpPost("{id}/submit")]
        public async Task<IActionResult> SubmitQuiz(int id, [FromBody] List<int> selectedAnswers)
        {
            var quiz = await _repo.GetQuizWithQuestions(id);
            if (quiz == null)
                return NotFound(new { message = "Quiz not found." });

            if (selectedAnswers.Count != quiz.Questions.Count)
                return BadRequest(new { message = "Invalid number of answers." });

            int score = 0;
            for (int i = 0; i < quiz.Questions.Count; i++)
            {
                var question = quiz.Questions[i];
                var correctAnswer = question.Answers.FirstOrDefault(a => a.IsCorrect);

                // Kommentar: Sjekker hvert svar mot riktig svar
                if (correctAnswer != null && selectedAnswers[i] == question.Answers.IndexOf(correctAnswer))
                {
                    score++;
                }
            }

            // Kommentar: Returnerer score og total for frontend
            return Ok(new { score, total = quiz.Questions.Count });
        }
    }
}
