namespace StudentFinance.Domain.Entities
{
    public class Obligation
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public bool IsPaid { get; set; }
        public Guid FamilyId { get; set; }

        // Dual currency support for future obligations
        public decimal LocalAmount { get; set; }
        public string LocalCurrency { get; set; } = string.Empty;

        public decimal FamilyAmount { get; set; }
        public string FamilyCurrency { get; set; } = string.Empty;
        public Family? Family { get; set; }
    }
}
