using Moq;
using ExpenseTracker.Infrastructure.Persistence.Repositories.Interfaces;
using ExpenseTracker.Domain.Services.Interfaces;
using ExpenseTracker.Application.Services;
using ExpenseTracker.UnitTests.Helpers.Entities;
using ExpenseTracker.Domain.Models.Entities;

namespace ExpenseTracker.UnitTests.Services
{
    public class ExpenseServiceTests
    {
        private readonly Mock<IExpenseRepository> _mockExpenseRepository;
        private readonly Mock<IDateRangeService> _mockDateRangeService;
        private readonly ExpenseService _expenseService;

        public ExpenseServiceTests()
        {
            _mockExpenseRepository = new Mock<IExpenseRepository>();
            _mockDateRangeService = new Mock<IDateRangeService>();
            _expenseService = new ExpenseService(_mockExpenseRepository.Object, _mockDateRangeService.Object);
        }

        [Fact]
        public async Task CreateExpenseAsync_WithValidDto_ShouldCreateExpense()
        {
            // Arrange
            var user = CreateUserEntities.User();
            var expenseDto = CreateExpenseEntities.CreateExpenseDto();

            _mockExpenseRepository.Setup(repo => repo.AddAsync(It.IsAny<Expense>(), It.IsAny<CancellationToken>()));

            // Act
            var result = await _expenseService.CreateExpenseAsync(expenseDto, user.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expenseDto.Amount, result.Amount);
            Assert.Equal(expenseDto.Category, result.Category);
            _mockExpenseRepository.Verify(repo => repo.AddAsync(It.IsAny<Expense>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateExpenseAsync_WithInvalidDto_ShouldThrowException()
        {
            // Arrange
            var user = CreateUserEntities.User();
            var expenseDto = CreateExpenseEntities.CreateExpenseDto("InvalidCategory");

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _expenseService.CreateExpenseAsync(expenseDto, user.Id));
        }

        [Fact]
        public async Task GetExpensesByUserAsync_WithValidUserId_ShouldReturnExpenses()
        {
            // Arrange
            var user = CreateUserEntities.User();
            var expenses = new List<Expense>
            {
                CreateExpenseEntities.Expense(user.Id, ExpenseCategory.Food),
                CreateExpenseEntities.Expense(user.Id, ExpenseCategory.Health)
            };

            // Mocking date range service behavior
            _mockDateRangeService.Setup(service => service.Normalize(It.IsAny<DateOnly>()))
                .Returns((DateOnly date) => date);

            _mockDateRangeService.Setup(service => service.GetMonthRange(It.IsAny<DateOnly>(), It.IsAny<int>()))
                .Returns((DateOnly date, int months) => (date.AddMonths(-months), date));

            // Mocking expense repository behavior
            _mockExpenseRepository.Setup(repo => repo.GetAsync(It.IsAny<Guid>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((Guid userId, DateOnly start, DateOnly end, int page, int pageSize) => (expenses, expenses.Count));

            // Act
            var result = await _expenseService.GetExpensesAsync(user.Id, DateOnly.FromDateTime(DateTime.Today), false, 1, 999);

            // Assert
            Assert.Equal(2, result.Items.Count);
            Assert.Equal("Food", result.Items[0].Category);
            Assert.Equal("Health", result.Items[1].Category);
        }

        [Fact]
        public async Task GetExpensesByUserAsync_WithNoUserId_ShouldThrowArgumentException()
        {
            // Arrange
            var EmptyUserId = Guid.Empty;

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _expenseService.GetExpensesAsync(EmptyUserId, null, false, 1, 10));
        }

        [Fact]
        public async Task UpdateExpenseAsync_WithValidData_ShouldUpdateExpense()
        {
            // Arrange
            var user = CreateUserEntities.User();
            var expense = CreateExpenseEntities.Expense(user.Id, ExpenseCategory.Food);
            var updateDto = CreateExpenseEntities.CreateExpenseDto("Health");

            _mockExpenseRepository.Setup(repo => repo.GetByIdAsync(expense.Id, user.Id))
                .ReturnsAsync(expense);

            _mockExpenseRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Expense>()));

            // Act
            await _expenseService.UpdateExpenseAsync(expense.Id, updateDto, user.Id);

            // Assert
            _mockExpenseRepository.Verify(repo => repo.GetByIdAsync(expense.Id, user.Id), Times.Once);
            _mockExpenseRepository.Verify(repo => repo.UpdateAsync(It.Is<Expense>(e =>
                e.Id == expense.Id &&
                e.Category == ExpenseCategory.Health &&
                e.Amount == 100)), Times.Once);
        }

        [Fact]
        public async Task DeleteExpenseAsync_WithValidExpense_ShouldDeleteExpense()
        {
            // Arrange
            var user = CreateUserEntities.User();
            var expense = CreateExpenseEntities.Expense(user.Id, ExpenseCategory.Food);

            _mockExpenseRepository.Setup(repo => repo.GetByIdAsync(expense.Id, user.Id))
                .ReturnsAsync(expense);

            _mockExpenseRepository.Setup(repo => repo.DeleteAsync(It.IsAny<Expense>()))
                .Returns(Task.CompletedTask);

            // Act
            await _expenseService.DeleteExpenseAsync(expense.Id, user.Id);

            // Assert
            _mockExpenseRepository.Verify(repo => repo.GetByIdAsync(expense.Id, user.Id), Times.Once);
            _mockExpenseRepository.Verify(repo => repo.DeleteAsync(It.Is<Expense>(e => e.Id == expense.Id)), Times.Once);
        }
    }

}