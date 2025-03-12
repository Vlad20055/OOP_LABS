using Lab1.Domain.Users;

namespace Lab1.Domain.Repositories
{
    internal interface IManagerRepository : IUserRepository<Manager>
    {
        public Task<List<Client>> ReadAllNotApprovedClientsAsync(CancellationToken cancellationToken);
        public Task ApproveClientAsync(Client client, CancellationToken cancellationToken);
    }
}
