using api.DAL;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.DAL
{
 // Repository responsible for all data operations related to Quiz entities.
// Implements IQuizRepository and uses EF Core to interact with the database.
    public class QuizRepository : IQuizRepository
    {
        private readonly AppDbContext _db;
        private readonly ILogger<QuizRepository> _logger;

        public QuizRepository(AppDbContext db, ILogger<QuizRepository> logger)
        {
            _db = db;
            _logger = logger;
        }
        // Retrieves all quizzes including related category
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
        // Retrieves a quiz by ID.
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
        // Retrieves a quiz with all related questions and options/answer 
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
        // Creates a new quiz in the database.
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
        // Updates an existing quiz.
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
        // Deletes a quiz and all its related questions and options/answers
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
        // Retrieves all quizzes created by a specific user.
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