namespace StudentFinance.Domain.Entities
{
    public class FinancialPlan
    {
        public Guid Id { get; set; }
        public string PlanName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalProjectedBudget { get; set; } // Calculated total cost
        public Guid StudentId { get; set; }
        public User? Student { get; set; }
    }
}
