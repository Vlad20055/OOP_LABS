using Lab1.Domain.BankServices;
using Lab1.Domain.Users;

namespace Lab1.Domain.Repositories
{
    internal interface IManagerRepository : IRepository<Manager>
    {
        public Task<List<Client>> ReadAllNotApprovedClientsAsync(CancellationToken cancellationToken);
        public Task ApproveClientAsync(Client client, CancellationToken cancellationToken);
        public Task ApproveClientBankAbilityAsync(BankAbility bankAbility, CancellationToken cancellationToken);
    }
}
