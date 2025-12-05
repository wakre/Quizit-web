using api.Models;

namespace api.DAL
{
    //Interface defining all database operations for Question entities
    public interface IQuestionRepository
    {
        Task<IEnumerable<Question>?> GetAll();// Retrieves all questions with their answers.
        Task<Question?> GetById(int questionId); // Retrieves a single question by its ID.
        
        Task<Question?> Create(Question question); //create a new question 
        Task<Question?> Update(Question question); // update existing question 
        Task<bool> Delete(int questionId); //delete question with its related answres options
        Task<Question?> GetWithAnswers(int questionId); //Retrieves a question with its answers and related quiz.
    }
}