using Lab1.Domain.Users;

namespace Lab1.Domain.Repositories
{
    internal interface IClientRepository : IRepository<Client>
    {
        public Task CreateAccountAsync(Account account, CancellationToken cancellationToken);
        public Task DeleteAccountAsync(Account account, CancellationToken cancellationToken);
        public Task AddClientAccountRecordAsync(Client client, Account account, CancellationToken cancellationToken);
        public Task<List<Account>> ReadAccountsByClientAsync(string login, CancellationToken cancellationToken);
        public Task RemoveClientAccountRecordAsync(Client client, Account account, CancellationToken cancellationToken);
        public Task AddAccountBankRecordAsync(Account account, Bank bank, CancellationToken cancellationToken);
        public Task RemoveAccountBankRecordAsync(Account account, Bank bank, CancellationToken cancellationToken);
    }
}
