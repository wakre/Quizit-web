using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using api.Controllers;
using api.DAL;
using api.DTOs;
using api.Models;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace api.Tests
{
    public class QuizControllerTests
    {
        private readonly Mock<IQuizRepository> _mockRepo;
        private readonly Mock<ILogger<QuizController>> _mockLogger;
        private readonly QuizController _controller;

        public QuizControllerTests()
        {
            _mockRepo = new Mock<IQuizRepository>();
            _mockLogger = new Mock<ILogger<QuizController>>();
            _controller = new QuizController(_mockRepo.Object, _mockLogger.Object);

            // Sett default User med userId = 1 for alle [Authorize]-tester
            _controller.ControllerContext.HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext
            {
                User = new ClaimsPrincipal(
                    new ClaimsIdentity(new Claim[]
                    {
                        new Claim("userId", "1")
                    }, "mock")
                )
            };
        }

        // GET ALL: Positive Test
        [Fact]
        public async Task GetAll_ReturnsOk_WithQuizDtos()
        {
            var quizzes = new List<Quiz>
            {
                new Quiz { QuizId = 1, Title = "Test Quiz", UserId = 1, CategoryId = 1, Category = new Category { Name = "Science" }, User = new User { UserName = "TestUser" } }
            };
            _mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(quizzes);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var dtos = Assert.IsAssignableFrom<List<QuizDto>>(okResult.Value);
            Assert.Single(dtos);
            Assert.Equal("Test Quiz", dtos[0].Title);
        }

        // GET BY ID: Positive Test
        [Fact]
        public async Task GetById_ReturnsOk_WithQuizDto()
        {
            var quiz = new Quiz
            {
                QuizId = 1,
                Title = "Test Quiz",
                UserId = 1,
                CategoryId = 1,
                Category = new Category { Name = "Science" },
                User = new User { UserName = "TestUser" },
                Questions = new List<Question> { new Question { QuestionId = 1, Text = "Q1", Answers = new List<Answer>() } }
            };
            _mockRepo.Setup(repo => repo.GetQuizWithQuestions(1)).ReturnsAsync(quiz);

            var result = await _controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsAssignableFrom<QuizDto>(okResult.Value);
            Assert.Equal(1, dto.QuizId);
        }

        // GET BY ID: Negative Test
        [Fact]
        public async Task GetById_ReturnsNotFound_WhenQuizNotExists()
        {
            _mockRepo.Setup(repo => repo.GetQuizWithQuestions(1)).ReturnsAsync((Quiz)null);

            var result = await _controller.GetById(1);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        // CREATE: Positive Test
        [Fact]
        public async Task Create_ReturnsOk_WithSuccessMessage()
        {
            var dto = new QuizCreateDto { Title = "New Quiz", CategoryId = 1 };
            var createdQuiz = new Quiz { QuizId = 1, Title = "New Quiz", UserId = 1 };
            _mockRepo.Setup(r => r.Create(It.IsAny<Quiz>())).ReturnsAsync(createdQuiz);

            var result = await _controller.Create(dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            dynamic response = okResult.Value!;
            Assert.Equal("Quiz created successfully!", response.message);
        }

        // CREATE: Negative Test
        [Fact]
        public async Task Create_ReturnsStatusCode500_WhenCreateFails()
        {
            var dto = new QuizCreateDto { Title = "New Quiz", CategoryId = 1 };
            _mockRepo.Setup(r => r.Create(It.IsAny<Quiz>())).ReturnsAsync((Quiz)null);

            var result = await _controller.Create(dto);

            var statusResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
        }

        // UPDATE: Positive Test
        [Fact]
        public async Task Update_ReturnsOk_WithSuccessMessage()
        {
            var dto = new QuizUpdateDto { Title = "Updated Quiz", CategoryId = 1 };
            var existingQuiz = new Quiz { QuizId = 1, Title = "Old Quiz", UserId = 1 };
            _mockRepo.Setup(r => r.GetById(1)).ReturnsAsync(existingQuiz);
            _mockRepo.Setup(r => r.Update(It.IsAny<Quiz>())).ReturnsAsync(existingQuiz);

            var result = await _controller.Update(1, dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            dynamic response = okResult.Value!;
            Assert.Equal("Quiz updated successfully!", response.message);
        }

        // UPDATE: Negative Test
        [Fact]
        public async Task Update_ReturnsNotFound_WhenQuizNotExists()
        {
            var dto = new QuizUpdateDto { Title = "Updated Quiz", CategoryId = 1 };
            _mockRepo.Setup(r => r.GetById(1)).ReturnsAsync((Quiz)null);

            var result = await _controller.Update(1, dto);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        // DELETE: Positive Test
        [Fact]
        public async Task Delete_ReturnsOk_WithSuccessMessage()
        {
            var existingQuiz = new Quiz { QuizId = 1, UserId = 1 };
            _mockRepo.Setup(r => r.GetById(1)).ReturnsAsync(existingQuiz);
            _mockRepo.Setup(r => r.Delete(1)).ReturnsAsync(true);

            var result = await _controller.Delete(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            dynamic response = okResult.Value!;
            Assert.Equal("Quiz deleted successfully!", response.message);
        }

        // DELETE: Negative Test
        [Fact]
        public async Task Delete_ReturnsNotFound_WhenQuizNotExists()
        {
            _mockRepo.Setup(r => r.GetById(1)).ReturnsAsync((Quiz)null);

            var result = await _controller.Delete(1);

            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
