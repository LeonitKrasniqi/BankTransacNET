using AccountManagmentSystemAPI.Data;
using AccountManagmentSystemAPI.Model.Domain;
using Microsoft.EntityFrameworkCore;

namespace AccountManagmentSystemAPI.Repositories
{
    public class SQLAccountRepository : IAccountRepository
    {
        private readonly FinancialDbContext dbContext;

        public SQLAccountRepository(FinancialDbContext dbContext) 
        {
            this.dbContext = dbContext;
        }

        public async Task<Account> CreateAsync(Account account)
        {
          await dbContext.Accounts.AddAsync(account);
            await dbContext.SaveChangesAsync();
            return account;
        }

        public async Task<Account> DeleteAsync(Guid id)
        {
            var existingAccount = await dbContext.Accounts.FirstOrDefaultAsync(x => x.AccountId == id);
            if (existingAccount==null)
            {
                return null;
                
            }
             dbContext.Accounts.Remove(existingAccount);
           await dbContext.SaveChangesAsync();
            return existingAccount;
        }

        public async Task<List<Account>> GetAllAsync()
        {
          return await dbContext.Accounts.ToListAsync();
        }

        public async Task<Account> GetByIdAsync(Guid id)
        {
            return await dbContext.Accounts.FirstOrDefaultAsync(x => x.AccountId == id);
        }

       
    }
}
