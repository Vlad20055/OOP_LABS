using Lab1.Application.Services;
using Lab1.Domain;
using Lab1.Domain.Users;
using Lab1.Infrastructure.Repositories;

Console.WriteLine("Hello, World!");

User user = new User();
user.Role = UserRole.Manager;
Console.WriteLine(user.Role);

ClientService clientService = new ClientService(new ClientRepository());
ManagerService managerService = new ManagerService(new ManagerRepository());
BankService bankService = new BankService(new BankRepository());

Client clientVladislav = clientService.Register(
    "Maksimenkov",
    "Vladislav",
    "Aleksandrovich",
    "Passport",
    "ID1",
    "+375333955034",
    "maksimenkovvlad111@mail.ru",
    "Vlad20055",
    "1234"
    );

Manager managerEkaterina = managerService.AddManager(
    "ID1",
    "Ekaterina",
    "Katia2006",
    "1234"
    );

Bank Belarusbank = bankService.Register("Belarusbank");



managerEkaterina.ApproveClient(clientVladislav);

Account account = clientVladislav.AddAccount(Belarusbank) ?? new Account();

clientVladislav = clientService.ReadClient("Vlad20055") ?? throw new Exception("No such Client");

foreach (var acc in clientVladislav.Accounts)
{
    Console.WriteLine(acc.IdNumber);
}





//bankService.Unregister(Belarusbank);
//managerService.DeleteManager(managerEkaterina);
//clientService.DeleteClient(clientVladislav);




