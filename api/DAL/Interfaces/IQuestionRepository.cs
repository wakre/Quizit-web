using api.Models;

namespace api.DAL
{
    public interface IQuestionRepository
    {
        Task<IEnumerable<Question>?> GetAll();
        Task<Question?> GetById(int questionId);
        Task<Question?> Create(Question question);
        Task<Question?> Update(Question question);
        Task<bool> Delete(int questionId);
        Task<Question?> GetWithAnswers(int questionId);
    }
}