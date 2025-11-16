using api.DAL;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.DAL
{
    public class QuizRepository : IQuizRepository
    {
        private readonly AppDbContext _db;
        private readonly ILogger<QuizRepository> _logger;

        public QuizRepository(AppDbContext db, ILogger<QuizRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        // GET ALL
        public async Task<IEnumerable<Quiz>?> GetAll()
        {
            try
            {
                _logger.LogInformation("[QuizRepository] Getting all quizzes...");

                return await _db.Quizzes
                    .Include(q => q.Category)
                    .Include(q => q.User)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[QuizRepository] Error in GetAll");
                return null;
            }
        }

        // GET BY ID 
        public async Task<Quiz?> GetById(int quizId)
        {
            try
            {
                return await _db.Quizzes
                    .Include(q => q.Category)
                    .Include(q => q.User)
                    .FirstOrDefaultAsync(q => q.QuizId == quizId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[QuizRepository] Error in GetById");
                return null;
            }
        }

        // GET WITH QUESTIONS & ANSWERS
        public async Task<Quiz?> GetQuizWithQuestions(int quizId)
        {
            try
            {
                return await _db.Quizzes
                    .Include(q => q.Category)
                    .Include(q => q.User)
                    .Include(q => q.Questions)
                        .ThenInclude(q => q.Answers)
                    .FirstOrDefaultAsync(q => q.QuizId == quizId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[QuizRepository] Error retrieving quiz with questions");
                return null;
            }
        }

        // CREATE
        public async Task<bool> Create(Quiz quiz)
        {
            try
            {
                _db.Quizzes.Add(quiz);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[QuizRepository] Error in Create");
                return false;
            }
        }

        // UPDATE
        public async Task<bool> Update(Quiz quiz)
        {
            try
            {
                _db.Quizzes.Update(quiz);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[QuizRepository] Error in Update");
                return false;
            }
        }

        // DELETE
        public async Task<bool> Delete(int quizId)
        {
            try
            {
                var quiz = await _db.Quizzes
                    .Include(q => q.Questions)
                        .ThenInclude(a => a.Answers)
                    .FirstOrDefaultAsync(q => q.QuizId == quizId);

                if (quiz == null)
                    return false;

                _db.Quizzes.Remove(quiz);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[QuizRepository] Error in Delete");
                return false;
            }
        }
    }
}
