using api.Models;

namespace api.DAL
{
    public interface IQuizRepository
    {
        Task<IEnumerable<Quiz>?> GetAll();
        Task<Quiz?> GetById(int quizId);
        Task<Quiz?> Create(Quiz quiz);
        Task<Quiz?> Update(Quiz quiz);
        Task<bool> Delete(int quizId);
        Task<Quiz?> GetQuizWithQuestions(int quizId);
        Task<IEnumerable<Quiz>?> GetQuizzesByUser(int userId);
    }
}