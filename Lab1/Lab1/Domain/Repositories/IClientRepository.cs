using Lab1.Domain.BankServices;
using Lab1.Domain.Users;

namespace Lab1.Domain.Repositories
{
    internal interface IClientRepository : IRepository<Client>
    {

        /*********************************************ACCOUNT*********************************************/

        public Task CreateAccountAsync(Account account, CancellationToken cancellationToken);
        public Task DeleteAccountAsync(Account account, CancellationToken cancellationToken);
        public Task AddClientAccountRecordAsync(Client client, Account account, CancellationToken cancellationToken);
        public Task<List<Account>> ReadAccountsByClientAsync(string login, CancellationToken cancellationToken);
        public Task RemoveClientAccountRecordAsync(Client client, Account account, CancellationToken cancellationToken);
        public Task AddAccountBankRecordAsync(Account account, Bank bank, CancellationToken cancellationToken);
        public Task RemoveAccountBankRecordAsync(Account account, Bank bank, CancellationToken cancellationToken);


        /*********************************************CREDIT*********************************************/

        public Task CreateCreditAsync(Credit credit, CancellationToken cancellationToken);
        public Task AddClientCreditRecordAsync(Client client, Credit credit, CancellationToken cancellationToken);
        public Task RemoveClientCreditRecordAsync(Client client, Credit credit, CancellationToken cancellationToken);
        public Task DeleteCreditAsync(Credit credit, CancellationToken cancellationToken);
        public Task<List<Credit>> ReadCreditsByClientAsync(string Login, CancellationToken cancellationToken);

    }
}
