using Lab1.Domain.BankServices;
using Lab1.Domain.Users;

namespace Lab1.Domain.Repositories
{
    internal interface ICompanySpecialistRepository : IRepository<CompanySpecialist>
    {
        public Task CreateSalaryProjectAsync(SalaryProject salaryProject, CancellationToken cancellationToken);
        public Task DeleteSalaryProjectAsync(SalaryProject salaryProject, CancellationToken cancellationToken);
        public Task<Company?> ReadCompanyAsync(string name, CancellationToken cancellationToken);
        public Task UpdateCompanyAsync(int companyId, int salaryProjectId, CancellationToken cancellationToken);
        public Task<SalaryProjectRequest?> ReadSalaryProjectRequestAsync(string login, string companyName, CancellationToken cancellationToken);
        public Task ApproveSalaryProjectRequestAsync(SalaryProjectRequest salaryProjectRequest, CancellationToken cancellationToken);
        public Task AddSalaryAsync(SalaryProjectRequest salaryProjectRequest, SalaryProject salaryProject, CancellationToken cancellationToken);
    }
}
