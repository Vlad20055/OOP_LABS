using Lab1.Domain.BankServices;

namespace Lab1.Domain.Repositories
{
    internal interface ICompanySpecialistRepository
    {
        public Task CreateSalaryProjectAsync(SalaryProject salaryProject, CancellationToken cancellationToken);
        public Task DeleteSalaryProjectAsync(SalaryProject salaryProject, CancellationToken cancellationToken);

    }
}
