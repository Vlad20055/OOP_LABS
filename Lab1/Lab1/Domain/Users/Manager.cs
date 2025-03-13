using Lab1.Domain.Repositories;

namespace Lab1.Domain.Users
{
    internal class Manager(IManagerRepository managerRepository) : User
    {
        public string IdNumber { get; set; }
        public string Name { get; set; }

        public async Task ApproveClient(Client client)
        {
            await managerRepository.ApproveClientAsync(client, CancellationToken.None);
            client.IsApproved = true;
        }
    }
}
