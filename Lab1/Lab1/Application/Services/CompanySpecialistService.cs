using Lab1.Application.Interfaces.Services;
using Lab1.Domain;
using Lab1.Domain.Repositories;
using Lab1.Domain.Users;
using Lab1.Infrastructure.Repositories;

namespace Lab1.Application.Services
{
    internal class CompanySpecialistService(ICompanySpecialistRepository companySpecialistRepository) : ICompanySpecialistService
    {
        public CompanySpecialist AddCompanySpecialist(
            string login,
            string password,
            string idNumber,
            string name,
            Company company
        )
        {

            var readingTask = companySpecialistRepository.ReadAsync(login, CancellationToken.None);
            Task.WaitAny(readingTask);
            CompanySpecialist? companySpecialist = readingTask.Result;

            if (companySpecialist != null)
            {
                Console.WriteLine("\nREGISTRATION ERROR!\nCompanySpecialist already exists!\n");
                return companySpecialist;
            }

            companySpecialist = new CompanySpecialist(companySpecialistRepository)
            {
                IdNumber = idNumber,
                Name = name,
                Login = login,
                Password = password,
                Role = UserRole.CompanySpecialist,
                Company = company
            };

            var creationTask = companySpecialistRepository.CreateAsync(companySpecialist, CancellationToken.None);
            creationTask.Wait();
            return companySpecialist;
        }

        public void DeleteCompanySpecialist(CompanySpecialist companySpecialist)
        {
            var deletingTask = companySpecialistRepository.DeleteAsync(companySpecialist, CancellationToken.None);
            deletingTask.Wait();
        }
    }
}
