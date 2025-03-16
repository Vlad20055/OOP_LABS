

using Lab1.Domain.BankServices;
using Lab1.Domain.Repositories;

namespace Lab1.Domain.Users
{
    internal class CompanySpecialist(ICompanySpecialistRepository companySpecialistRepository)
    {
        public required Company Company { get; set; } // shows what Company CompanySpecialist is related to

        public SalaryProject AddSalaryProject(Company company)
        {
            SalaryProject salaryProject = new SalaryProject()
            {
                BankId = company.BankIdentificationCode,
                CompanyName = company.Name,
            };

            var creatingTask = companySpecialistRepository.CreateSalaryProjectAsync(salaryProject, CancellationToken.None);
            creatingTask.Wait();

            return salaryProject;
        }
    }
}
