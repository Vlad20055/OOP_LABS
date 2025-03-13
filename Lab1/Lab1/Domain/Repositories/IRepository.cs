

namespace Lab1.Domain.Repositories
{
    internal interface IRepository<T>
    {
        public Task CreateAsync(T entity, CancellationToken cancellationToken);
        public Task<T?> ReadAsync(string login, CancellationToken cancellationToken); // returns entity
        public Task DeleteAsync(T entity, CancellationToken cancellationToken);
    }
}
