using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentFinance.Application.DTOs;
using StudentFinance.Application.Interfaces.Services;

namespace StudentFinance.API.Controllers
{
    [ApiController]
    [Route("api/families")]
    [Authorize]
    public class FamilyController : ControllerBase
    {
        private readonly IFamilyService _familyService;
        private readonly IProjectedExpensesService _projectedExpensesService;

        public FamilyController(
            IFamilyService familyService,
            IProjectedExpensesService projectedExpensesService)
        {
            _familyService = familyService;
            _projectedExpensesService = projectedExpensesService;
        }

        [HttpGet("{familyId}/summary")]
        public async Task<IActionResult> GetFamilySummary(
            Guid familyId,
            CancellationToken cancellationToken)
        {
            var summary = await _familyService.GetFamilySummaryAsync(familyId, cancellationToken);

            return Ok(summary);
        }

        [HttpGet("{familyId}/projected-expenses")]
        public async Task<IActionResult> GetProjectedExpenses(
            Guid familyId,
            CancellationToken cancellationToken)
        {
            var projection = await _projectedExpensesService
                .CalculateNextMonthProjectionAsync(familyId, cancellationToken);

            return Ok(new ProjectedExpenseResponse
            {
                ProjectedAmount = projection
            });
        }
    }
}
