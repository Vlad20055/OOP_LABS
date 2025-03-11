using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lab1.Application.Interfaces.Repositories;
using Lab1.Application.Interfaces.Services;
using Lab1.Domain.Users;
using Lab1.Infrastructure;

namespace Lab1.Application.Services
{
    internal class ManagerService(IManagerRepository managerRepository) : IManagerService
    {
        public void AddManager(
            string idNumber,
            string name)
        {
            Manager manager = new Manager() {
                IdNumber = idNumber,
                Name = name };

            var creationTask = managerRepository.CreateAsync(manager, CancellationToken.None);
            Task.WaitAny(creationTask);
        }

        public void DeleteManager(Manager manager)
        {
            var deletingTask = managerRepository.DeleteAsync(manager, CancellationToken.None);
            Task.WaitAny(deletingTask);
        }

        public void ApproveClient(Client client)
        {
            var approvalTask = managerRepository.UpdateClientAsync(client, CancellationToken.None);
            Task.WaitAny(approvalTask);
        }
    }
}
