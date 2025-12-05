using api.Models;

namespace api.DAL
{
    // Interface defining all database operations for Quiz entities.
    public interface IQuizRepository
    {
        Task<IEnumerable<Quiz>?> GetAll(); // Retrieves all quizzes including category.
        
        Task<Quiz?> GetById(int quizId); // Retrieves a quiz by its ID.        
        Task<Quiz?> Create(Quiz quiz);//creates a new quiz 
        Task<Quiz?> Update(Quiz quiz); // update existing quiz 
        Task<bool> Delete(int quizId); //delete existing quiz with its related  questions ans options 
        Task<Quiz?> GetQuizWithQuestions(int quizId); // Retrieves a quiz with its questions and answers
        Task<IEnumerable<Quiz>?> GetQuizzesByUser(int userId); // Retrieves quizzes created by a specific user
    }
}