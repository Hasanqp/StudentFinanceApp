using StudentFinance.Domain.Enums;

namespace StudentFinance.Application.DTOs
{
    public class CreateObligationRequest
    {
        public string Title { get; set; } = string.Empty;
        public decimal LocalAmount { get; set; }
        public Currency LocalCurrency { get; set; }
        public DateTime DueDate { get; set; }
    }
}
