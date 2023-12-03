using AccountManagmentSystemAPI.Model.Domain;

namespace AccountManagmentSystemAPI.Repositories
{
    public interface IAccountRepository
    {
        Task<List<Account>> GetAllAsync();
        Task<Account> GetByIdAsync(Guid id);
        Task<Account> CreateAsync(Account account);

        Task<Account> DeleteAsync(Guid id);
    }
}
