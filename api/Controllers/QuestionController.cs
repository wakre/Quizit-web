using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.DAL;
using api.Models;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionController : ControllerBase
    {
        private readonly AppDbContext _db;
        public QuestionController(AppDbContext db)
        {
           _db = db; 
        }
        //Creating Question
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Question dto)
        {
            var quiz = await _db.Quizzes.FirstOrDefaultAsync(quiz => quiz.QuizId == dto.QuizId);
            if(quiz == null)
               return NotFound();

            var userId = int.Parse(User.FindFirst("UserId")!.Value);
            if (quiz.UserId !=userId)
                return Unauthorized(new {message ="You dont have access to add question to the Quiz"}); 

                _db.Questions.Add(dto);
                await _db.SaveChangesAsync();

                return Ok(new{message = "the question added successfully! "});

        }
        
    }
}