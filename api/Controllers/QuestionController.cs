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
        private readonly IQuizRepository _quizRepo;
        private readonly ILogger<QuestionController> _logger;

        public QuestionController(IQuestionRepository repo, IQuizRepository quizRepo, ILogger<QuestionController> logger)
        {
            _repo = repo;
            _quizRepo = quizRepo;
            _logger = logger;
        }

        // CREATE: Legg til nytt spørsmål med 2–4 svar
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] QuestionCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Hent quiz for å sjekke eierskap og antall spørsmål
            var quiz = await _quizRepo.GetQuizWithQuestions(dto.QuizId);
            if (quiz == null)
                return NotFound(new { message = "Quiz not found." });

            var userId = int.Parse(User.FindFirst("userId")!.Value);
            if (quiz.UserId != userId)
                return Unauthorized(new { message = "You don't have access to add questions to this quiz." });

            if (quiz.Questions.Count >= 10)
                return BadRequest(new { message = "Quiz cannot have more than 10 questions." });

            // MANUELL MAPPING: DTO → Question entity
            var question = new Question
            {
                Text = dto.Text,
                QuizId = dto.QuizId,
                // Map hver option til Answer entity og sett riktig svar
                Answers = dto.Options.Select((opt, index) => new Answer
                {
                    Text = opt,
                    IsCorrect = index == dto.CorrectOptionIndex
                }).ToList()
            };

            var created = await _repo.Create(question);
            if (created == null)
                return StatusCode(500, "Error creating question.");

            // RETURNERER: QuestionId til frontend
            return Ok(new { message = "Question added successfully!", questionId = created.QuestionId });
            
        }

        // UPDATE: Oppdater eksisterende spørsmål
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

            // MANUELL MAPPING: oppdater spørsmålstekst
            question.Text = dto.Text;

            // MANUELL MAPPING: oppdater alle svar og korrekt indeks
            for (int i = 0; i < question.Answers.Count; i++)
            {
                question.Answers[i].Text = dto.Options[i];
                question.Answers[i].IsCorrect = i == dto.CorrectOptionIndex;
            }

            var updated = await _repo.Update(question);
            if (updated == null)
                return StatusCode(500, "Error updating question.");

            return Ok(new { message = "Question updated successfully!" });
        }

        // DELETE: Slett spørsmål (kun quiz-eier)
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

        // GET: Hent spørsmål med svar (frontend)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var question = await _repo.GetWithAnswers(id);
            if (question == null)
                return NotFound(new { message = "Question not found." });

            // MANUELL MAPPING: Question entity → QuestionDto for frontend
            var questionDto = new QuestionDto
            {
                QuestionId = question.QuestionId,
                Text = question.Text,
                UserId = question.Quiz.UserId,
                Answers = question.Answers.Select(a => new AnswerDto
                {
                    AnswerId = a.AnswerId,
                    Text = a.Text,
                    IsCorrect = a.IsCorrect
                }).ToList()
            };

            return Ok(questionDto);
        }
    }
}
