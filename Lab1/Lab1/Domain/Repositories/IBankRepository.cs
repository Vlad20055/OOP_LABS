

namespace Lab1.Domain.Repositories
{
    internal interface IBankRepository : IRepository<Bank>
    {
        public Task<Bank> ReadAsync(int id, CancellationToken cancellationToken);
    }
}
