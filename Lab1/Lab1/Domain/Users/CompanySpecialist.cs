using Lab1.Domain.BankServices;
using Lab1.Domain.Repositories;

namespace Lab1.Domain.Users
{
    internal class CompanySpecialist(ICompanySpecialistRepository companySpecialistRepository) : User
    {
        public required string IdNumber { get; set; }
        public required string Name { get; set; }
        public required Company Company { get; set; } // shows what Company CompanySpecialist is related to

        public void AddSalaryProject()
        {
            if (Company.SalaryProject != null)
            {
                Console.WriteLine("\nERROR!\nSalaryProject already exists\n");
                return;
            }

            var salaryProject = new SalaryProject()
            {
                BankId = Company.Account.Bank.Id,
                CompanyName = Company.Name
            };

            Company.SalaryProject = salaryProject;

            var creatingTask = companySpecialistRepository.CreateSalaryProjectAsync(salaryProject, CancellationToken.None);
            creatingTask.Wait();
            var updatingCompanyTask = companySpecialistRepository.UpdateCompanyAsync(Company.Id, salaryProject.IdNumber, CancellationToken.None);
            updatingCompanyTask.Wait();
            
        }

        public override string ToString()
        {
            return $"CompanySpecialist: [Login: {Login}, Name: {Name}, IdNumber: {IdNumber}, Company: {Company.Name}]";
        }
    }
}
