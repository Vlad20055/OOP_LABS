using Lab1.Domain.Repositories;

namespace Lab1.Domain.Users
{
    internal class Client(IClientRepository clientRepository) : User
    {
        public string? Surname { get; set; }
        public string? Name { get; set; }
        public string? Patronymic { get; set; }
        public string? PassportSeriesAndNumber { get; set; }
        public string? IdNumber { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public bool IsApproved { get; set; } = false;

        public List<Bank> Banks { get; set; } = new List<Bank>();
        public List<Account> Accounts { get; set; } = new List<Account>();


        public Account? AddAccount(Bank bank)
        {
            if (!IsApproved)
            {
                Console.WriteLine("\nERROR!\nClient is not approved\n");
                return null;
            }

            Account account = new Account()
            {
                Bank = bank,
                Amount = 0,
                IsActive = true,
                Client = this
            };

            var creationTask = clientRepository.CreateAccountAsync(account, CancellationToken.None);

            if (!Banks.Contains(bank)) Banks.Add(bank);
            Accounts.Add(account);

            Task.WaitAny(creationTask);
            return account;
        }
    }
}
