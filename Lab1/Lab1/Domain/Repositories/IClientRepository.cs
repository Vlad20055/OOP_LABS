using Lab1.Domain.Users;

namespace Lab1.Domain.Repositories
{
    internal interface IClientRepository : IRepository<Client>
    {
        public Task CreateAccountAsync(Account account, CancellationToken cancellationToken);
    }
}
