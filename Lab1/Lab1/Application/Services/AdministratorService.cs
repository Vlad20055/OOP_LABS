using Lab1.Application.Interfaces.Services;
using Lab1.Domain.Repositories;
using Lab1.Domain.Users;
using Lab1.Infrastructure.Repositories;

namespace Lab1.Application.Services
{
    internal class AdministratorService(IAdministratorRepository administratorRepository) : IAdministratorService
    {
        public Administrator AddAdministrator(string login, string password, string idNumber, string name)
        {
            var readingTask = administratorRepository.ReadAsync(login, CancellationToken.None);
            readingTask.Wait();
            Administrator? administrator = readingTask.Result;

            if (administrator != null)
            {
                Console.WriteLine("\nERROR\nAdministrator already exists!\n");
                return administrator;
            }

            administrator = new Administrator(new TransferRepository())
            {
                Login = login,
                Password = password,
                IdNumber = idNumber,
                Name = name,
                Role = UserRole.Administrator
            };

            administratorRepository.CreateAsync(administrator, CancellationToken.None).Wait();
            return administrator;
        }

        public void DeleteAdministrator(Administrator administrator)
        {
            administratorRepository.DeleteAsync(administrator, CancellationToken.None).Wait();
        }
    }
}
