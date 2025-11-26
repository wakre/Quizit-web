using Microsoft.EntityFrameworkCore;
using api.Models;

namespace api.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Quiz → Questions (1-many)
            modelBuilder.Entity<Quiz>()
                .HasMany(q => q.Questions)
                .WithOne(q => q.Quiz)
                .HasForeignKey(q => q.QuizId)
                .OnDelete(DeleteBehavior.Cascade);

            // Question → Answers (1-many)
            modelBuilder.Entity<Question>()
                .HasMany(q => q.Answers)
                .WithOne(a => a.Question)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            // User → Quizzes (1-many)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Quizzes)
                .WithOne(q => q.User)
                .HasForeignKey(q => q.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Category → Quizzes (1-many)
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Quizzes)
                .WithOne(q => q.Category)
                .HasForeignKey(q => q.CategoryId)
                .OnDelete(DeleteBehavior.Restrict); // Prevents deleting categories with quizzes

            // Seed initial categories (for demo)
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Science" },
                new Category { CategoryId = 2, Name = "Family" },
                new Category { CategoryId = 3, Name = "Sports" },
                new Category { CategoryId = 4, Name = "Math" },
                new Category { CategoryId = 5, Name = "Other" }

            );
            //a sample user 
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId= 1,
                    UserName="TestUser",
                    Email= "testUser@example.com",
                    PasswordHash=BCrypt.Net.BCrypt.HashPassword("Password2025")
                }
             );
            //a sample quiz 
            modelBuilder.Entity<Quiz>().HasData(
                new Quiz
                {
                    QuizId=1, 
                    Title="Test your Math knowledge",
                    Description="a simple math quiz to test your knowledge in Math, Good Luck!!",
                    DateCreated= DateTime.UtcNow,
                    UserId=1,
                    CategoryId= 4 

                }
            );
            // a sample question for quiz 1
            modelBuilder.Entity<Question>().HasData(
                new Question
                {
                    QuestionId =1, 
                    Text= "What is 4 * 4 =??",
                    QuizId = 1
                },
                new Question
                {
                    QuestionId =2, 
                    Text= "What is 4 * 5 =??",
                    QuizId = 1 
                },
                new Question
                {
                    QuestionId =3, 
                    Text= "What is 5 * 5 =??",
                    QuizId = 1 
                }
            );
            // a sample answer
            modelBuilder.Entity<Answer>().HasData(
                new Answer
                { //answers for question 1: 
                    AnswerId= 1, 
                    Text= "14",
                    IsCorrect= false, 
                    QuestionId=1
                },
                new Answer
                {
                    AnswerId= 2, 
                    Text= "25",
                    IsCorrect= false, 
                    QuestionId=1
                },
                new Answer
                {
                    AnswerId= 3, 
                    Text= "16",
                    IsCorrect= true, 
                    QuestionId=1
                },
                new Answer
                {//answers for question 2: 
                    AnswerId= 4, 
                    Text= "14",
                    IsCorrect= false, 
                    QuestionId=2
                },
                new Answer
                {
                    AnswerId= 5, 
                    Text= "20",
                    IsCorrect= true, 
                    QuestionId=2
                },
                 new Answer
                {
                    AnswerId= 6, 
                    Text= "28",
                    IsCorrect= false, 
                    QuestionId=2
                },
                new Answer
                { //answers for question 3: 
                    AnswerId= 7, 
                    Text= "55",
                    IsCorrect= false, 
                    QuestionId=3
                },
                new Answer
                {
                    AnswerId= 8, 
                    Text= "28",
                    IsCorrect= false, 
                    QuestionId=3
                },
                new Answer
                {
                    AnswerId= 9, 
                    Text= "25",
                    IsCorrect= true, 
                    QuestionId=3
                }   
            );

        }
    }
}