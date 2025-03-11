using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Application.Interfaces.Repositories
{
    internal interface IRepository<T>
    {
        public Task<int> CreateAsync(T entity, CancellationToken cancellationToken);
        public Task DeleteAsync(T entity, CancellationToken cancellationToken);
    }
}
