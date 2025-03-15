using Lab1.Application.Services;
using Lab1.Domain;
using Lab1.Domain.Users;
using Lab1.Domain.BankServices;
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
    ) ?? throw new Exception();

Manager managerEkaterina = managerService.AddManager(
    "ID1",
    "Ekaterina",
    "Katia2006",
    "1234"
    );

Bank Belarusbank = bankService.Register("Belarusbank");



//await managerEkaterina.ApproveClient(clientVladislav);
//clientVladislav.AddAccount(Belarusbank);
//clientVladislav.AddCredit(Belarusbank, CreditPeriod.Month_3, 2, 100);
//clientVladislav = clientService.ReadClient("Vlad20055") ?? throw new Exception("No such Client");
await managerEkaterina.ApproveCredit(clientVladislav.Credits[0]);

Console.WriteLine("Accounts:");
foreach (var acc in clientVladislav.Accounts)
{
    Console.WriteLine(acc.ToString());
}

Console.WriteLine("Credits:");
foreach (var cred in clientVladislav.Credits)
{
    Console.WriteLine(cred.ToString());
}

//clientVladislav.DeleteCredit(clientVladislav.Credits[0]);
//clientVladislav.DeleteAccount(clientVladislav.Accounts[0]);

//bankService.Unregister(Belarusbank); // Doesn't work correctly!
//managerService.DeleteManager(managerEkaterina);
//clientService.DeleteClient(clientVladislav);





