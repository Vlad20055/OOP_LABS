﻿using Lab1.Application.Services;
using Lab1.Domain;
using Lab1.Domain.Users;
using Lab1.Infrastructure.Repositories;

Console.WriteLine("Hello, World!");

User user = new User();
user.Role = UserRole.Manager;
Console.WriteLine(user.Role);

ClientService clientService = new ClientService(new ClientRepository());
ManagerService managerService = new ManagerService(new ManagerRepository());

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

managerEkaterina.ApproveClient(clientVladislav);

Account account = clientVladislav.AddAccount(new Bank()) ?? new Account();
Console.WriteLine(account.IdNumber);

//managerService.DeleteManager(managerEkaterina);
//clientService.DeleteClient(clientVladislav);




