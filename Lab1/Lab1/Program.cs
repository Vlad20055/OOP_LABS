using Lab1.Application.Services;
using Lab1.Domain.Users;
using Lab1.Infrastructure;

Console.WriteLine("Hello, World!");

User user = new User();
user.Role = UserRole.Manager;
Console.WriteLine(user.Role);

ClientService clientService = new ClientService(new ClientRepository());
ManagerService managerService = new ManagerService(new ManagerRepository());

clientService.Register(
    "Maksimenkov",
    "Vladislav",
    "Aleksandrovich",
    "Passport",
    "ID1",
    "+375333955034",
    "maksimenkovvlad111@mail.ru"
    );

managerService.AddManager(
    "ID1",
    "Anna"
    );



List<Client> NotApprovedClients = clientService.GetAllNotApprovedClients();

foreach (Client client in NotApprovedClients)
{
    managerService.ApproveClient(client);
}

NotApprovedClients = clientService.GetAllNotApprovedClients();

int cnt = 1;
foreach(Client client in NotApprovedClients)
{
    Console.WriteLine($"{cnt} {client.Name}");
    ++cnt;
}

foreach (Client client in NotApprovedClients)
{
    clientService.DeleteClient(client);
}

