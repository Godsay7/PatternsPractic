using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace DAL
{
    public class FinanceDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public FinanceDbContext(DbContextOptions<FinanceDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Налаштування для Account
            modelBuilder.Entity<Account>().HasKey(a => a.Id);
            modelBuilder.Entity<Account>().Property(a => a.Name).IsRequired().HasMaxLength(100);

            // Налаштування для Category
            modelBuilder.Entity<Category>().HasKey(c => c.Id);
            modelBuilder.Entity<Category>().Property(c => c.Name).IsRequired();

            // Налаштування для Transaction та зв'язків
            modelBuilder.Entity<Transaction>().HasKey(t => t.Id);

            // Зв'язок: Транзакція має один Рахунок, Рахунок має багато Транзакцій
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Account)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.AccountId);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Category)
                .WithMany(c => c.Transactions)
                .HasForeignKey(t => t.CategoryId);
        }
    }
}
