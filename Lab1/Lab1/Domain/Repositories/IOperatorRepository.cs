

using System.Buffers;
using Lab1.Domain.BankServices;
using Lab1.Domain.Users;

namespace Lab1.Domain.Repositories
{
    internal interface IOperatorRepository : IRepository<Operator>
    {
        public Task CompleteSalaryProjectAsync(SalaryProject salaryProject, CancellationToken cancellationToken);
    }
}
