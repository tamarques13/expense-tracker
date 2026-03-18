using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Spentir.Services.Interfaces;
using Spentir.Controllers.Base;
using Spentir.DTOs;

namespace Spentir.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/expenses")]
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
            var expenseDto = await _expenseService.CreateExpenseAsync(dto, Guid.Parse(UserId));

            return CreatedAtAction(nameof(GetExpenses), expenseDto);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<IActionResult> GetExpenses(DateOnly? date, bool isLastYear)
        {
            var expensesDto = await _expenseService.GetExpensesAsync(Guid.Parse(UserId), date, isLastYear);

            return Ok(expensesDto);            
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut]
        public async Task<IActionResult> UpdateExpense(Guid Id, CreateExpenseDto dto)
        {
            await _expenseService.UpdateExpenseAsync(Id, dto, Guid.Parse(UserId));

            return NoContent();
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete]
        public async Task<IActionResult> DeleteExpense(Guid expenseId)
        {
            await _expenseService.DeleteExpenseAsync(expenseId, Guid.Parse(UserId));

            return NoContent();
        }
    }
}