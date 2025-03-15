using Lab1.Application.Interfaces.Services;
using Lab1.Domain;
using Lab1.Domain.BankServices;
using Lab1.Domain.Repositories;
using Lab1.Domain.Users;

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
                readingTask.Wait();
                
                Client client = readingTask.Result ?? throw new Exception();

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
                creationTask.Wait();

                return client;
            }
        }

        public Client? ReadClient(string login)
        {
            var readingTask = clientRepository.ReadAsync(login, CancellationToken.None);
            readingTask.Wait();
            return readingTask.Result;
        }

        public void DeleteClient(Client client)
        {
            //Delete all accounts
            {
                var accounts = new List<Account>(client.Accounts);

                foreach (var acc in accounts)
                {
                    client.DeleteAccount(acc);
                }
            }

            //Delete all credits
            {
                var credits = new List<Credit>(client.Credits);

                foreach (var cred in credits)
                {
                    client.DeleteCredit(cred);
                }
            }
            

            var deletingTask = clientRepository.DeleteAsync(client, CancellationToken.None);
            deletingTask.Wait();
        }
    }
}
