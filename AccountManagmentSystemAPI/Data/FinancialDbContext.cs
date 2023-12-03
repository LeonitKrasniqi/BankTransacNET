using AccountManagmentSystemAPI.Model.Domain;
using Microsoft.EntityFrameworkCore;

namespace AccountManagmentSystemAPI.Data
{
    public class FinancialDbContext : DbContext
    {

        public FinancialDbContext(DbContextOptions<FinancialDbContext> dbContextOptions) : base(dbContextOptions)
        {
            
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasKey(a => a.AccountId);

            modelBuilder.Entity<Account>()
                .HasIndex(a => a.CardNumber)
                .IsUnique();

            modelBuilder.Entity<Transaction>()
                .HasKey(t => t.TransactionId);

            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount)
                .IsRequired();

            modelBuilder.Entity<Transaction>()
                .Property(t => t.TransactionDate)
                .IsRequired();


            
            
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Account)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.SenderId)
                .OnDelete(DeleteBehavior.Cascade); 


            modelBuilder.Entity<Transaction>()
           .HasOne(t => t.Account)
            .WithMany(a => a.Transactions)
           .HasForeignKey(t => t.ReceiverId)
           .OnDelete(DeleteBehavior.Cascade);
           



            base.OnModelCreating(modelBuilder);
        }


    }
}
