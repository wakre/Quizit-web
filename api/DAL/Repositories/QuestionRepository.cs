using api.DAL;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.DAL
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly AppDbContext _db;
        private readonly ILogger<QuestionRepository> _logger;

        public QuestionRepository(AppDbContext db, ILogger<QuestionRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<IEnumerable<Question>?> GetAll()
        {
            try
            {
                return await _db.Questions
                    .Include(q => q.Answers)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[QuestionRepository] Error in GetAll");
                return null;
            }
        }

        public async Task<Question?> GetById(int questionId)
        {
            try
            {
                return await _db.Questions
                    .Include(q => q.Answers)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(q => q.QuestionId == questionId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[QuestionRepository] Error in GetById");
                return null;
            }
        }

        public async Task<Question?> GetWithAnswers(int questionId)
        {
            try
            {
                return await _db.Questions
                    .Include(q => q.Answers)
                    .Include(q => q.Quiz)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(q => q.QuestionId == questionId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[QuestionRepository] Error in GetWithAnswers");
                return null;
            }
        }

        public async Task<Question?> Create(Question question)
        {
            try
            {
                _db.Questions.Add(question);
                await _db.SaveChangesAsync();
                return question;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[QuestionRepository] Error in Create");
                return null;
            }
        }

        public async Task<Question?> Update(Question question)
        {
            try
            {
                _db.Questions.Update(question);
                await _db.SaveChangesAsync();
                return question;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[QuestionRepository] Error in Update");
                return null;
            }
        }

        public async Task<bool> Delete(int questionId)
        {
            try
            {
                var q = await _db.Questions
                    .Include(a => a.Answers)
                    .FirstOrDefaultAsync(q => q.QuestionId == questionId);

                if (q == null) return false;

                _db.Questions.Remove(q);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[QuestionRepository] Error in Delete");
                return false;
            }
        }
    }
}