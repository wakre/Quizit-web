using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using api.DAL;
using api.Models;
using api.DTOs;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuizController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ILogger<QuizController> _logger;
        public QuizController(AppDbContext db, ILogger<QuizController> logger)
        {
            _db = db;
            _logger = logger; 
        
        }
        //----------- get all ----------
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var quizzes = await _db.Quizzes
                .Include(q => q.Category)
                .Include(q => q.User)
                .AsNoTracking()
                .ToListAsync();
            return Ok(quizzes);
        }

        //------------- get by id---------------
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var quiz = await _db.Quizzes
            .Include(q => q.Category)
            .Include(q => q.Questions)
                .ThenInclude(quiz => quiz.Answers)
            .FirstOrDefaultAsync(quiz => quiz.QuizId == id);

            if (quiz == null)
                return NotFound(new {message = "the Quiz is not Found"});
            return Ok(quiz);
        }
        //------ creating with innloging--------------
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] QuizCreateDto dto)
        {
            var userId = int.Parse(User.FindFirst("userId")!.Value);
            var quiz = new Quiz
            {
                Title = dto.Title,
                Description = dto.Description, 
                ImageUrl = dto.ImageUrl, 
                CategoryId = dto.CategoryId, 
                UserId = userId,
                DateCreated = DateTime.UtcNow
            };
            _db.Quizzes.Add(quiz);
            await _db.SaveChangesAsync();
            return Ok (new{message = " Quiz created successfully!!", quizId =quiz.QuizId});

        }
        //------update quiz, owner only-----------
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] QuizUpdateDto dto)
        {
            var quiz = await _db.Quizzes.FindAsync(id);
            if (quiz ==null)
                return NotFound();
            var userId = int.Parse(User.FindFirst("userId")!.Value);
            if (quiz.UserId != userId)
                return Unauthorized(new {message="You dont have rights to Update this Quiz"});
            
            quiz.Title= dto.Title;
            quiz.Description= dto.Description;
            quiz.ImageUrl= dto.ImageUrl;
            quiz.CategoryId= dto.CategoryId;

            await _db.SaveChangesAsync();
            return Ok(new { message= "Quiz updated!"});

        }
        //----------delete quiz, owner only -----------¨
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var quiz= await _db.Quizzes
                .Include(q => q.Questions)
                .ThenInclude(quiz => quiz.Answers)
                .FirstOrDefaultAsync(quiz => quiz.QuizId == id);
            
            if (quiz == null)
                return NotFound();

            var userId = int.Parse(User.FindFirst("userId")!.Value);
            if (quiz.UserId != userId)
                return Unauthorized(new {message= "you dont have the rights to delete this quiz"});

            _db.Quizzes.Remove(quiz);
            await _db.SaveChangesAsync();
            return Ok(new{message= "Quiz deleted successfully!"});
        }

    }
}