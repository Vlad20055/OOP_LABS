using Lab1.Application.Services;
using Lab1.Domain;
using Lab1.Domain.BankServices;
using Lab1.Domain.Users;
using Lab1.Infrastructure.Repositories;



Console.WriteLine("Hello, World!");


ClientService clientService = new ClientService(new ClientRepository(), new TransferRepository());
ManagerService managerService = new ManagerService(new ManagerRepository());
BankService bankService = new BankService(new BankRepository());
CompanyService companyService = new CompanyService(new CompanyRepository());
CompanySpecialistService companySpecialistService = new CompanySpecialistService(new CompanySpecialistRepository());


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

Client clientAndrey = clientService.Register(
    "Lubashenko",
    "Andrey",
    "Sergeevich",
    "Passport",
    "ID2",
    "@Lolipop",
    "Lolipop@mail.ru",
    "Andrey2006",
    "1234"
    ) ?? throw new Exception();

Manager managerEkaterina = managerService.AddManager(
    "ID1",
    "Ekaterina",
    "Katia2006",
    "1234"
    );

Bank Belarusbank = bankService.Register(
    "Belarusbank"
    );

Company Microsoft = companyService.Register(
    "Microsoft",
    CompanyType.LimitedLiabilityCompany,
    "USA",
    Belarusbank
    );

CompanySpecialist specialistAnton = companySpecialistService.AddCompanySpecialist("Anton2006", "1234", "ID1", "Anton", Microsoft);
//specialistAnton.AddSalaryProject();
specialistAnton.ApproveSalaryProjectRequest(clientVladislav);
specialistAnton.ApproveSalaryProjectRequest(clientAndrey);

//await managerEkaterina.ApproveClient(clientVladislav);
//await managerEkaterina.ApproveClient(clientAndrey);
//clientVladislav.AddAccount(Belarusbank);
//clientAndrey.AddAccount(Belarusbank);
//clientVladislav.AddCredit(Belarusbank, Period.Month_3, 2, 100);
//clientVladislav.AddInstallment(Belarusbank, Period.Month_24, 1.5m, 300);
//clientVladislav.AddDeposit(Belarusbank, Period.Month_12, 2.5m, 1000);
//clientVladislav.AddSalaryProjectRequest(Microsoft, 1000, clientVladislav.Accounts[0]);
//clientAndrey.AddSalaryProjectRequest(Microsoft, 1000, clientAndrey.Accounts[0]);
//clientVladislav = clientService.ReadClient("Vlad20055") ?? throw new Exception("No such Client");
//await managerEkaterina.ApproveCredit(clientVladislav.Credits[0]);
//await managerEkaterina.ApproveInstallment(clientVladislav.Installments[0]);
//await managerEkaterina.ApproveDeposit(clientVladislav.Deposits[0]);
//clientAndrey.Transfer(clientAndrey.Accounts[0], clientVladislav.Accounts[0], 50);
//clientVladislav.Transfer(clientVladislav.Accounts[0], clientAndrey.Accounts[0], 50);

Console.WriteLine("\nClient VLADISLAV\n");
Console.WriteLine("Accounts:");
foreach (var acc in clientVladislav.Accounts)
{
    Console.WriteLine(acc.ToString());
}
Console.WriteLine();

Console.WriteLine("Credits:");
foreach (var cred in clientVladislav.Credits)
{
    Console.WriteLine(cred.ToString());
}
Console.WriteLine();

Console.WriteLine("Installments:");
foreach (var inst in clientVladislav.Installments)
{
    Console.WriteLine(inst.ToString());
}
Console.WriteLine();

Console.WriteLine("Deposits:");
foreach (var dep in clientVladislav.Deposits)
{
    Console.WriteLine(dep.ToString());
}


Console.WriteLine("\nClient ANDREY\n");
Console.WriteLine("Accounts:");
foreach (var acc in clientAndrey.Accounts)
{
    Console.WriteLine(acc.ToString());
}
Console.WriteLine();

Console.WriteLine("Credits:");
foreach (var cred in clientAndrey.Credits)
{
    Console.WriteLine(cred.ToString());
}
Console.WriteLine();

Console.WriteLine("Installments:");
foreach (var inst in clientAndrey.Installments)
{
    Console.WriteLine(inst.ToString());
}
Console.WriteLine();

Console.WriteLine("Deposits:");
foreach (var dep in clientAndrey.Deposits)
{
    Console.WriteLine(dep.ToString());
}


Console.WriteLine("\n\n\nCOMPANY\n\n\n");
Console.WriteLine(Microsoft.ToString());


Console.WriteLine("\n\n\nCOMPANY SPECIALIST\n\n\n");
Console.WriteLine(specialistAnton.ToString());

//clientVladislav.DeleteAccount(clientVladislav.Accounts[0]);
//clientVladislav.DeleteCredit(clientVladislav.Credits[0]);
//clientVladislav.DeleteInstallment(clientVladislav.Installments[0]);
//clientVladislav.DeleteDeposit(clientVladislav.Deposits[0]);

//bankService.Unregister(Belarusbank); // Doesn't work correctly!
//companyService.Unregister(Microsoft);
//managerService.DeleteManager(managerEkaterina);
//clientService.DeleteClient(clientVladislav);





