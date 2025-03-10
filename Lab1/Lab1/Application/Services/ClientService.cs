using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lab1.Application.Interfaces.Repositories;
using Lab1.Domain.Users;

namespace Lab1.Application.Services
{
    internal class ClientService(IClientRepository clientRepository)
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
            Client client = new Client();

            client.Surname = surname;
            client.Name = name;
            client.Patronymic = patronymic;
            client.PassportSeriesAndNumber = passportSeriesAndNumber;
            client.IdNumber = idNumber;
            client.PhoneNumber = phoneNumber;
            client.Email = email;

            // create some accounts for him
            // add some banks for him

            clientRepository.CreateAsync(client, CancellationToken.None);
        }
    }
}
