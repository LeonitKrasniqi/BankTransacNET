using AccountManagmentSystemAPI.Data;
using AccountManagmentSystemAPI.Model.Domain;
using Microsoft.EntityFrameworkCore;

namespace AccountManagmentSystemAPI.Repositories
{
    public class SQLTransactionRepository : ITransactionRepository
    {
        private readonly FinancialDbContext dbContext;

        public SQLTransactionRepository(FinancialDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> TransferMoneyAsync(Guid senderAccountId, Guid receiverAccountId, decimal amount)
        {

            using var transaction = dbContext.Database.BeginTransaction();
            

            try
            {

                if (senderAccountId == receiverAccountId)
                {
                    return false;
                }



                var senderAccount = await dbContext.Accounts
                    .Include(a => a.Transactions)
                    .FirstOrDefaultAsync(a => a.AccountId == senderAccountId);

                var receiverAccount = await dbContext.Accounts
                    .Include(a => a.Transactions)
                    .FirstOrDefaultAsync(a => a.AccountId == receiverAccountId);


                if(senderAccount == null || receiverAccount == null)
                {
                    return false;
                }

                if(senderAccount.Balance < amount)
                {
                    return false;
                }

                //Create transaction for sender
                var senderTransaction = new Transaction
                {
                    Amount = -amount,
                    TransactionDate = DateTime.UtcNow,
                    IsDebit = true,
                    ReceiverId = receiverAccountId
                };

                //Create transaction for receiver
                var receiverTransaction = new Transaction
                {
                    Amount = amount,
                    TransactionDate = DateTime.UtcNow,
                    IsDebit = false,
                    SenderId = senderAccountId,
                };



                //Update account balances
                senderAccount.Balance -= amount;
                receiverAccount.Balance += amount;


                //Add transactoin to accounts
                senderAccount.Transactions.Add(senderTransaction);
                receiverAccount.Transactions.Add(receiverTransaction);

                await dbContext.SaveChangesAsync();
                transaction.Commit();

                return true;

            }

            catch(Exception)
            {
                transaction.Rollback();

           
                return false;
            }

        }
    }
}
