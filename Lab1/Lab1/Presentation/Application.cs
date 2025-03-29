using System.Runtime.InteropServices;
using System.Security;
using Lab1.Application.Interfaces.Services;
using Lab1.Application.Services;
using Lab1.Domain;
using Lab1.Domain.Users;
using Lab1.Infrastructure.Repositories;


namespace Lab1.Presentation
{
    internal class Application
    {
        public Dictionary<string, Object> Services { get; set; } = new Dictionary<string, Object>();
        public LoginMenu? LoginMenu;

        public void Build()
        {
            Services["AdministratorService"] = new AdministratorService(new AdministratorRepository());

            LoginMenu = new LoginMenu()
            {
                IsRunning = true,
                Name = "login"
            };

            LoginMenu.LoginCompleted += OnLoginCompleted;
        }


        public void Run()
        {
            while (true)
            {
                if (LoginMenu != null) LoginMenu.Display();
                else
                {
                    Console.WriteLine("No LoginMenu!");
                    return;
                }
            }
        }

        public void OnLoginCompleted(User user)
        {
            if ((user as Administrator) != null)
            {
                var AdministratorMenu = new Menu
                {
                    IsRunning = true,
                    Name = "administrator",
                    Options =
                    {
                        ("See all transfers", new Action()
                        {
                            Name = "all transfers",
                            IsRunning = true,
                            Handler = () =>
                            {
                                Administrator administrator = (user as Administrator) ?? throw new Exception("No Administrator");
                                var transfers = administrator.GetAllTransfers();
                                foreach (var trans in transfers)
                                {
                                    Console.WriteLine(trans.ToString());
                                }
                            }
                        })
                    }
                };
                AdministratorMenu.Display();
            }
            if ((user as Client) != null)
            {
                var ClientMenu = new Menu
                {
                    IsRunning = true,
                    Name = "client",
                    Options =
                    {
                        ("Add Transfer", new Action()
                        {
                            Name = "Transfer",
                            IsRunning = true,
                            Handler = () =>
                            {
                                int senderAccountId;
                                int recipientAccountId;
                                int amount;
                                try
                                {
                                    Console.Write("Sender Account ID: ");
                                    senderAccountId = Convert.ToInt32(Console.ReadLine());
                                }
                                catch
                                {
                                    Console.WriteLine("No input");
                                    return;
                                }
                                
                                try
                                {
                                    Console.Write("Recipient Account ID: ");
                                    recipientAccountId = Convert.ToInt32(Console.ReadLine());
                                }
                                catch
                                {
                                    Console.WriteLine("No input");
                                    return;
                                }
                                try
                                {
                                    Console.Write("Amount: ");
                                    amount = Convert.ToInt32(Console.ReadLine());
                                }
                                catch
                                {
                                    Console.WriteLine("No input");
                                    return;
                                }
                                
                                if (amount == 0)
                                {
                                    Console.WriteLine("Anount can not be zero");
                                    return;
                                }
                                Client client = (user as Client) ?? throw new Exception("No Client");
                                Account senderAccount;
                                Account recipientAccount;
                                try
                                {
                                    senderAccount = client.GetAccount(senderAccountId);
                                    recipientAccount = client.GetAccount(recipientAccountId);
                                }
                                catch
                                {
                                    Console.WriteLine("No account");
                                    return;
                                }
                                client.Transfer(senderAccount, recipientAccount, amount);
                                return;
                            }
                        }),
                        ("Show all Accounts", new Action()
                        {
                            Name = "Accounts:",
                            IsRunning = true,
                            Handler = () =>
                            {
                                Client client = (user as Client) ?? throw new Exception("No Client");
                                var accounts = client.Accounts;
                                foreach (var acc in accounts)
                                {
                                    Console.WriteLine(acc.ToString());
                                }
                                return;
                            }
                        }),
                        ("Show all Deposits", new Action()
                        {
                            Name = "Deposits:",
                            IsRunning = true,
                            Handler = () =>
                            {
                                Client client = (user as Client) ?? throw new Exception("No Client");
                                foreach (var dep in client.Deposits)
                                {
                                    Console.WriteLine(dep.ToString());
                                }
                                return;
                            }
                        }),
                        ("Show all Credits", new Action()
                        {
                            Name = "Credits:",
                            IsRunning = true,
                            Handler = () =>
                            {
                                Client client = (user as Client) ?? throw new Exception("No Client");
                                foreach (var cred in client.Credits)
                                {
                                    Console.WriteLine(cred.ToString());
                                }
                                return;
                            }
                        })
                    }
                };
                ClientMenu.Display();
            }
            if ((user as CompanySpecialist) != null)
            {
                var CompanySpecialistMenu = new Menu
                {
                    IsRunning = true,
                    Name = "company specialist",
                    Options =
                    {
                        ("Show Company", new Action()
                        {
                            Name = "Company:",
                            IsRunning = true,
                            Handler = () =>
                            {
                                CompanySpecialist companySpecialist = (user as CompanySpecialist) ?? throw new Exception("No Administrator");
                                Console.WriteLine(companySpecialist.Company.ToString());
                            }
                        }
                        ),
                        ("Send salary project request", new Action()
                        {
                            Name = "Salary project request sent",
                            IsRunning = true,
                            Handler = () =>
                            {
                                CompanySpecialist companySpecialist = (user as CompanySpecialist) ?? throw new Exception("No Administrator");
                                companySpecialist.DecompleteSalaryProject();
                            }
                        })
                    }
                };
                CompanySpecialistMenu.Display();
            }

            if ((user as Operator) != null)
            {
                var OperatorMenu = new Menu
                {
                    IsRunning = true,
                    Name = "operator",
                    Options =
                    {
                        ("Realize salary project", new Action()
                        {
                            Name = "Realization salary project",
                            IsRunning = true,
                            Handler = () =>
                            {
                                string? name;
                                Company? company;
                                Operator @operator = (user as Operator) ?? throw new Exception("No Operator");
                                Console.Write("Company name: ");
                                name = Console.ReadLine();
                                if (name != null)
                                {
                                    company = new CompanyRepository().ReadAsync(name, CancellationToken.None).Result;
                                    if (company != null)
                                    {
                                        @operator.RealizeSalaryProject(company);
                                    }
                                    else
                                    {
                                        Console.WriteLine("No Company");
                                        return;
                                    }
                                    
                                }
                                else
                                {
                                    Console.WriteLine("No Company!");
                                    return;
                                }
                                
                            }
                        })
                    }
                };
                OperatorMenu.Display();
            }
        }
    }
}
