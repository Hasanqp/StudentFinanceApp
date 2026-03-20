using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentFinance.Application.DTOs;
using StudentFinance.Application.Interfaces.Services;
using System.Security.Claims;

namespace StudentFinance.API.Controllers
{
    [ApiController]
    [Route("api/expenses")]
    [Authorize]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;

        public ExpenseController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateExpense([FromBody] CreateExpenseRequest request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var studentIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (studentIdClaim == null)
                return Unauthorized();

            var studentId = Guid.Parse(studentIdClaim);

            var result = await _expenseService.CreateExpenseAsync(
                studentId,
                request.LocalAmount,
                request.LocalCurrency,
                request.Category,
                request.Note,
                cancellationToken
            );

            return CreatedAtAction(
                nameof(GetMonthlyExpenses),
                new { studentId = studentId },
                result
            );
        }

        [HttpGet("monthly")]
        public async Task<IActionResult> GetMonthlyExpenses([FromQuery] int month, [FromQuery] int year, CancellationToken cancellationToken)
        {
            var studentIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (studentIdClaim == null)
                return Unauthorized();

            var studentId = Guid.Parse(studentIdClaim);

            var result = await _expenseService
                .GetStudentMonthlyExpensesAsync(studentId, month, year, cancellationToken);

            return Ok(result);
        }
    }
}
