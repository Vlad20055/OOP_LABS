using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab1.Domain.Users;

namespace Lab1.Application.Interfaces.Services
{
    internal interface IManagerService
    {
        public void AddManager(
            string idNumber,
            string name);

        public void DeleteManager(Manager manager);

        public void ApproveClient(Client client);
    }
}
