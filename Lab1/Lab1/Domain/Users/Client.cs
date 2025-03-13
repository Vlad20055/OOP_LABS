using System.Numerics;
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
            Task.WaitAny(creationTask);
            var clientAccountTask = clientRepository.AddClientAccountRecordAsync(this, account, CancellationToken.None);
            Task.WaitAny(clientAccountTask);
            var accountBankTask = clientRepository.AddAccountBankRecordAsync(account, bank, CancellationToken.None);
            Task.WaitAny(accountBankTask);

            Accounts.Add(account);

            return account;
        }

        public void DeleteAccount(Account account)
        {
            if (!IsApproved)
            {
                Console.WriteLine("\nERROR!\nClient is not approved\n");
                return;
            }

            var deletingAccountBankTask = clientRepository.RemoveAccountBankRecordAsync(account, account.Bank, CancellationToken.None);
            Task.WaitAny(deletingAccountBankTask);
            var deletingClientAccountTask = clientRepository.RemoveClientAccountRecordAsync(this, account, CancellationToken.None);
            Task.WaitAny(deletingClientAccountTask);
            var deletingAccountTask = clientRepository.DeleteAccountAsync(account, CancellationToken.None);
            Task.WaitAny(deletingAccountTask);
        }
    }
}
