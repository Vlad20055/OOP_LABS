

namespace Lab1.Domain.Repositories
{
    internal interface ICompanyRepository : IRepository<Company>
    {
        public Task CreateAccountAsync(Account account, CancellationToken cancellationToken);
        public Task DeleteAccountAsync(Account account, CancellationToken cancellationToken);
        public Task AddAccountBankRecordAsync(Account account, Bank bank, CancellationToken cancellationToken);
        public Task RemoveAccountBankRecordAsync(Account account, Bank bank, CancellationToken cancellationToken);
    }
}
