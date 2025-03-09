using Lab1.Domain.Users;

Console.WriteLine("Hello, World!");

User user = new User();
user.Role = UserRole.Manager;
Console.WriteLine(user.Role);