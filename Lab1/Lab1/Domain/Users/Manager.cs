using Lab1.Domain.Repositories;

namespace Lab1.Domain.Users
{
    internal class Manager(IManagerRepository managerRepository) : User
    {
        public string IdNumber { get; set; }
        public string Name { get; set; }

        public void ApproveClient(Client client)
        {
            var approvalTask = managerRepository.ApproveClientAsync(client, CancellationToken.None);
            Task.WaitAny(approvalTask);
        }
    }
}
