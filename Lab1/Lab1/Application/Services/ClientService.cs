using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lab1.Application.Interfaces.Repositories;
using Lab1.Application.Interfaces.Services;
using Lab1.Domain.Users;

namespace Lab1.Application.Services
{
    internal class ClientService(IClientRepository clientRepository) : IClientService
    {
        public void Register(
            string surname,
            string name,
            string patronymic,
            string passportSeriesAndNumber,
            string idNumber,
            string phoneNumber,
            string email)
        {
            Client client = new Client()
            {
                Surname = surname,
                Name = name,
                Patronymic = patronymic,
                PassportSeriesAndNumber = passportSeriesAndNumber,
                IdNumber = idNumber,
                PhoneNumber = phoneNumber,
                Email = email
            };

            var creationTask = clientRepository.CreateAsync(client, CancellationToken.None);
            Task.WaitAny(creationTask);
        }

        public List<Client> GetAllNotApprovedClients()
        {
            var gettingTask = clientRepository.ReadAllNotApprovedAsync(CancellationToken.None);
            Task.WaitAny(gettingTask);

            return gettingTask.Result;
        }

        public void DeleteClient(Client client)
        {
            var deletingTask = clientRepository.DeleteAsync(client, CancellationToken.None);
            Task.WaitAny(deletingTask);
        }
    }
}
