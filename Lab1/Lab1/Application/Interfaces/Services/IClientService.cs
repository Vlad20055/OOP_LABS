using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab1.Domain.Users;

namespace Lab1.Application.Interfaces.Services
{
    internal interface IClientService
    {
        public void Register(
            string surname,
            string name,
            string patronymic,
            string passportSeriesAndNumber,
            string idNumber,
            string phoneNumber,
            string email);
        public List<Client> GetAllNotApprovedClients();
        public void DeleteClient(Client client);
    }
}
