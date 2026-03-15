using Microsoft.AspNetCore.Mvc;
using Spentir.Services.Interfaces;
using Spentir.DTOs;

namespace Spentir.Controllers
{
    [ApiController]
    [Route("api/expenses")]
    public class ExpenseController(IExpenseService expenseService) : ControllerBase
    {
        private readonly IExpenseService _expenseService = expenseService;

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> CreateExpense(CreateExpenseDto dto)
        {
            var expenseDto = await _expenseService.CreateExpenseAsync(dto);

            return CreatedAtAction(nameof(GetExpenses), expenseDto);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<IActionResult> GetExpenses()
        {
            var expensesDto = await _expenseService.GetExpensesAsync();

            return Ok(expensesDto);            
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut]
        public async Task<IActionResult> UpdateExpense(Guid Id, CreateExpenseDto dto)
        {
            await _expenseService.UpdateExpenseAsync(Id, dto);

            return NoContent();
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete]
        public async Task<IActionResult> DeleteExpense(Guid expenseId)
        {
            await _expenseService.DeleteExpenseAsync(expenseId);

            return NoContent();
        }
    }
}