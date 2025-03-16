using Lab1.Domain.BankServices;
using Lab1.Domain.Users;
using Lab1.Infrastructure.Options;
using Lab1.Infrastructure.Repositories;
using Npgsql;

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

        /********************************************BANK ABILITY*********************************************/

        public Task CreateBankAbilityAsync(BankAbility bankAbility, CancellationToken cancellationToken);
        public Task DeleteBankAbilityAsync(BankAbility bankAbility, CancellationToken cancellationToken);
        public Task AddClientBankAbilityRecordAsync(Client client, BankAbility bankAbility, CancellationToken cancellationToken);
        public Task RemoveClientBankAbilityRecordAsync(Client client, BankAbility bankAbility, CancellationToken cancellationToken);

        /*********************************************CREDIT*********************************************/

        public Task<List<Credit>> ReadCreditsByClientAsync(string Login, CancellationToken cancellationToken);

        /********************************************INSTALLMENT*********************************************/

        public Task<List<Installment>> ReadInstallmentsByClientAsync(string login, CancellationToken cancellationToken);

        /********************************************DEPOSIT*********************************************/

        public Task<List<Deposit>> ReadDepositsByClientAsync(string login, CancellationToken cancellationToken);
        
    }
}
