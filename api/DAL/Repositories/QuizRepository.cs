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

        public async Task<Quiz?> GetById(int quizId)
        {
            try
            {
                return await _db.Quizzes
                    .Include(q => q.Category)
                    .Include(q => q.User)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(q => q.QuizId == quizId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[QuizRepository] Error in GetById");
                return null;
            }
        }

        public async Task<Quiz?> GetQuizWithQuestions(int quizId)
        {
            try
            {
                return await _db.Quizzes
                    .Include(q => q.Category)
                    .Include(q => q.User)
                    .Include(q => q.Questions)
                        .ThenInclude(q => q.Answers)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(q => q.QuizId == quizId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[QuizRepository] Error retrieving quiz with questions");
                return null;
            }
        }

        public async Task<Quiz?> Create(Quiz quiz)
        {
            try
            {
                _db.Quizzes.Add(quiz);
                await _db.SaveChangesAsync();
                return quiz;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[QuizRepository] Error in Create");
                return null;
            }
        }

        public async Task<Quiz?> Update(Quiz quiz)
        {
            try
            {
                _db.Quizzes.Update(quiz);
                await _db.SaveChangesAsync();
                return quiz;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[QuizRepository] Error in Update");
                return null;
            }
        }

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

        public async Task<IEnumerable<Quiz>?> GetQuizzesByUser(int userId)
        {
            try
            {
                return await _db.Quizzes
                    .Where(q => q.UserId == userId)
                    .Include(q => q.Category)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[QuizRepository] Error in GetQuizzesByUser");
                return null;
            }
        }
    }
}