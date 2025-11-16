using api.Models;

namespace api.DAL
{
    public interface IQuizRepository
    {
        Task<IEnumerable<Quiz>?> GetAll();
        Task<Quiz?> GetById(int quizId);
        Task<bool> Create(Quiz quiz);
        Task<bool> Update(Quiz quiz);
        Task<bool> Delete(int quizId);
        Task<Quiz?> GetQuizWithQuestions(int quizId);
    }
}
