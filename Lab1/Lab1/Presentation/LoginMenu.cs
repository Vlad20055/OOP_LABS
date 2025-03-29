

using Lab1.Application.Services;
using Lab1.Domain.Users;
using Lab1.Infrastructure.Repositories;

namespace Lab1.Presentation
{
    internal class LoginMenu : IUiInstance
    {
        public string Name { get; set; } = "login";
        public bool IsRunning { get; set; } = false;

        public event Action<User>? LoginCompleted;
        public void Display()
        {
            IsRunning = true;

            while (IsRunning)
            {
                Console.Clear();
                Console.WriteLine($"\n=== {Name.ToUpper()} ===");
                Console.Write("Login: ");
                var login = Console.ReadLine();

                if (login != null)
                {
                    User? user = null;

                    user = new ClientRepository().ReadAsync(login, CancellationToken.None).Result;
                    if (user != null)
                    {
                        IsRunning = false;
                        LoginCompleted?.Invoke(user);
                        return;
                    }

                    user = new AdministratorRepository().ReadAsync(login, CancellationToken.None).Result;
                    if (user != null)
                    {
                        IsRunning = false;
                        LoginCompleted?.Invoke(user);
                        return;
                    }

                    user = new CompanySpecialistRepository().ReadAsync(login, CancellationToken.None).Result;
                    if (user != null)
                    {
                        IsRunning = false;
                        LoginCompleted?.Invoke(user);
                        return;
                    }

                    user = new OperatorRepository().ReadAsync(login, CancellationToken.None).Result;
                    if (user != null)
                    {
                        IsRunning = false;
                        LoginCompleted?.Invoke(user);
                        return;
                    }
                }
            }
        }
    }
}
