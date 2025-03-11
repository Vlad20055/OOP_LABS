using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lab1.Domain.Users;

namespace Lab1.Application.Interfaces.Repositories
{
    internal interface IManagerRepository : IRepository<Manager>
    {
        public Task UpdateClientAsync(Client client, CancellationToken cancellationToken);
    }
}
