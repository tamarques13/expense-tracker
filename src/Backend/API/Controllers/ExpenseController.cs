using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ExpenseTracker.Application.Services.Interfaces;
using ExpenseTracker.API.Controllers.Base;
using ExpenseTracker.Application.DTOs;

namespace ExpenseTracker.API.Controllers
{
    // [Authorize]
    [AllowAnonymous]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/expenses")]
    public class ExpenseController(IExpenseService expenseService) : BaseController
    {
        private readonly IExpenseService _expenseService = expenseService;

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> CreateExpense(CreateExpenseDto dto)
        {
            // var expenseDto = await _expenseService.CreateExpenseAsync(dto, Guid.Parse(UserId));
            var expenseDto = await _expenseService.CreateExpenseAsync(dto, Guid.Parse("3ba6b5b1-60d6-4179-9cae-9ba4bed6a39b"));

            return CreatedAtAction(nameof(GetExpenses), expenseDto);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<IActionResult> GetExpenses(DateOnly? date, bool isLastYear, int page = 1, int pageSize = 20)
        {
            // var expensesDto = await _expenseService.GetExpensesAsync(Guid.Parse(UserId), date, isLastYear, page, pageSize);
            var expensesDto = await _expenseService.GetExpensesAsync(Guid.Parse("3ba6b5b1-60d6-4179-9cae-9ba4bed6a39b"), date, isLastYear, page, pageSize);

            return Ok(expensesDto);
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExpense(Guid id, CreateExpenseDto dto)
        {
            // await _expenseService.UpdateExpenseAsync(id, dto, Guid.Parse(UserId));
            await _expenseService.UpdateExpenseAsync(id, dto, Guid.Parse("3ba6b5b1-60d6-4179-9cae-9ba4bed6a39b"));

            return NoContent();
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(Guid id)
        {
            // await _expenseService.DeleteExpenseAsync(id, Guid.Parse(UserId));
            await _expenseService.DeleteExpenseAsync(id, Guid.Parse("3ba6b5b1-60d6-4179-9cae-9ba4bed6a39b"));

            return NoContent();
        }
    }
}