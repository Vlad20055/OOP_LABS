using Lab1.Application.Interfaces.Services;
using Lab1.Domain.Repositories;
using Lab1.Domain.Users;
using Lab1.Infrastructure;
using Npgsql;

namespace Lab1.Application.Services
{
    internal class ManagerService(IManagerRepository managerRepository) : IManagerService
    {
        public Manager AddManager(
            string idNumber,
            string name,
            string login,
            string password
            )
        {
            
            var readingTask = managerRepository.ReadAsync(login, CancellationToken.None);
            Task.WaitAny(readingTask);
            Manager? manager = readingTask.Result;

            if (manager != null)
            {
                Console.WriteLine("\nREGISTRATION ERROR!\nManager already exists!\n");
                return manager;
            }

            manager = new Manager(managerRepository)
            {
                IdNumber = idNumber,
                Name = name,
                Login = login,
                Password = password,
                Role = UserRole.Manager
            };

            var creationTask = managerRepository.CreateAsync(manager, CancellationToken.None);
            Task.WaitAny(creationTask);

            return manager;
        }

        public void DeleteManager(Manager manager)
        {
            var deletingTask = managerRepository.DeleteAsync(manager, CancellationToken.None);
            Task.WaitAny(deletingTask);
        }
    }
}
