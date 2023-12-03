using AccountManagmentSystemAPI.Model.Domain;

namespace AccountManagmentSystemAPI.Repositories
{
    public interface ITransactionRepository
    {
         Task<bool> TransferMoneyAsync(Guid senderAccountId, Guid receiverAccountId, decimal amount);
    }
}
