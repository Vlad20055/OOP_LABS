using Lab1.Application.Interfaces.Services;
using Lab1.Domain.Repositories;
using Lab1.Domain.Users;
using Npgsql;

namespace Lab1.Application.Services
{
    internal class ClientService(IClientRepository clientRepository) : IClientService
    {
        public Client Register(
            string surname,
            string name,
            string patronymic,
            string passportSeriesAndNumber,
            string idNumber,
            string phoneNumber,
            string email,
            string login,
            string password
            )
        {
            try 
            {
                var readingTask = clientRepository.ReadAsync(login, CancellationToken.None);
                Task.WaitAny(readingTask);
                Client client = readingTask.Result;

                Console.WriteLine("\nREGISTRATION ERROR!\nClient already exists!\n");

                return client;
            }
            catch
            {
                Client client = new Client(clientRepository)
                {
                    Surname = surname,
                    Name = name,
                    Patronymic = patronymic,
                    PassportSeriesAndNumber = passportSeriesAndNumber,
                    IdNumber = idNumber,
                    PhoneNumber = phoneNumber,
                    Email = email,
                    Login = login,
                    Password = password,
                    Role = UserRole.Clilent
                };

                var creationTask = clientRepository.CreateAsync(client, CancellationToken.None);
                Task.WaitAny(creationTask);

                return client;
            }
        }

        public void DeleteClient(Client client)
        {
            var deletingTask = clientRepository.DeleteAsync(client, CancellationToken.None);
            Task.WaitAny(deletingTask);
        }
    }
}
