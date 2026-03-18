using Microsoft.EntityFrameworkCore;
using StudentFinance.Domain.Entities;

namespace StudentFinance.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Family> Families { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Obligation> Obligations { get; set; }
        public DbSet<FinancialPlan> FinancialPlans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Accurately configure relationships (Fluent API) to ensure data integrity

            // Family relationship with users
            modelBuilder.Entity<Family>()
                .HasMany(f => f.Members)
                .WithOne(u => u.Family)
                .HasForeignKey(u => u.FamilyId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting the family if it contains users

            // The relationship between the expense and the user (student)
            modelBuilder.Entity<Expense>()
                .HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // The relationship of expenses with the family
            modelBuilder.Entity<Expense>()
                .HasOne(e => e.Family)
                .WithMany(f => f.Expenses)
                .HasForeignKey(e => e.FamilyId)
                .OnDelete(DeleteBehavior.Cascade); // If you delete the family, you delete their expenses

            modelBuilder.Entity<Expense>()
                .Property(e => e.LocalCurrency)
                .HasConversion<string>();

            modelBuilder.Entity<Expense>()
                .Property(e => e.FamilyCurrency)
                .HasConversion<string>();

            modelBuilder.Entity<Family>()
                .Property(f => f.BaseCurrency)
                .HasConversion<string>();
        }
    }
}
