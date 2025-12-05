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

        //--------------GET ALL QUIZZES---------------
        //  public endpoint used for listing quizzes in guest mode
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var quizzes = await _repo.GetAll();
            if (quizzes == null)
                return StatusCode(500, "Error retrieving quizzes.");
            
            //Manual mapping: flatten category and user names for list display
            var quizDtos = quizzes.Select(q => new QuizDto
            {
                QuizId = q.QuizId,
                Title = q.Title,
                Description = q.Description,
                ImageUrl = q.ImageUrl,
                CategoryId = q.CategoryId,
                CategoryName = q.Category?.Name ?? "Unknown",  
                UserId = q.UserId,
                UserName = q.User?.UserName ?? "Unknown",     
                Questions = new List<QuestionDto>()           
            }).ToList();

            return Ok(quizDtos);
        }

        // ------------Get quiz by ID----------- 
        //public endpoint for reading or taking a quiz for all users include guest 
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var quiz = await _repo.GetQuizWithQuestions(id);
            if (quiz == null)
                return NotFound(new { message = "Quiz not found." });
            //Manual mapping: include questions + answers for quiz taking
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

        // -----------------Create quiz--------------
        // only authenticated users can create quizzes
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] QuizCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = int.Parse(User.FindFirst("userId")!.Value);

            // Manuell mapping:  DTO -> entity
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

            return Ok(new { message = "Quiz created successfully!", quizId = created.QuizId });
        }

        // --------------Update quiz--------------
        // only the quiz owner can update
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

            // Manuell mapping: update entity fields 
            quiz.Title = dto.Title;
            quiz.Description = dto.Description;
            quiz.ImageUrl = dto.ImageUrl;
            quiz.CategoryId = dto.CategoryId;

            var updated = await _repo.Update(quiz);
            if (updated == null)
                return StatusCode(500, "Error updating quiz.");

            return Ok(new { message = "Quiz updated successfully!" });
        }

        // ---------------Delete quiz-------------
        //only the owner can delete their quiz
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

            return Ok(new { message = "Quiz deleted successfully!" });
        }

        //-------------------Submit quiz answers and calculate score-------------
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

                if (correctAnswer != null && selectedAnswers[i] == question.Answers.IndexOf(correctAnswer))
                {
                    score++;
                }
            }

            return Ok(new { score, total = quiz.Questions.Count });
        }
    }
}
