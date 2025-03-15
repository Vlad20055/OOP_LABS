using System.Data;
using Lab1.Domain.BankServices;
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
        public List<Credit> Credits { get; set; } = new List<Credit>();


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
                IsActive = true
            };

            var creationTask = clientRepository.CreateAccountAsync(account, CancellationToken.None);
            creationTask.Wait();
            var clientAccountTask = clientRepository.AddClientAccountRecordAsync(this, account, CancellationToken.None);
            var accountBankTask = clientRepository.AddAccountBankRecordAsync(account, bank, CancellationToken.None);
            
            Task.WaitAll(clientAccountTask,  accountBankTask);

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
            deletingAccountBankTask.Wait();
            var deletingClientAccountTask = clientRepository.RemoveClientAccountRecordAsync(this, account, CancellationToken.None);
            deletingClientAccountTask.Wait();
            var deletingAccountTask = clientRepository.DeleteAccountAsync(account, CancellationToken.None);
            deletingAccountTask.Wait();

            Accounts.Remove(account);
        }

        public Credit? AddCredit(Bank bank, CreditPeriod creditPeriod, decimal percent, decimal rest)
        {
            if (!IsApproved)
            {
                Console.WriteLine("\nERROR!\nClient is not approved\n");
                return null;
            }

            Credit credit = new Credit()
            {
                Bank = bank,
                IsApproved = false,
                Period = creditPeriod,
                Persent = percent,
                Rest = rest
            };

            var creatingTask = clientRepository.CreateCreditAsync(credit, CancellationToken.None);
            creatingTask.Wait();
            var clientCreditTask = clientRepository.AddClientCreditRecordAsync(this, credit, CancellationToken.None);
            clientCreditTask.Wait();

            Credits.Add(credit);

            return credit;
        }

        public void DeleteCredit(Credit credit)
        {
            if (!IsApproved)
            {
                Console.WriteLine("\nERROR!\nClient is not approved\n");
                return;
            }

            var deletingTask = clientRepository.DeleteCreditAsync(credit, CancellationToken.None);
            deletingTask.Wait();
            var deletingClientCreditTask = clientRepository.RemoveClientCreditRecordAsync(this, credit, CancellationToken.None);
            deletingClientCreditTask.Wait();

            Credits.Remove(credit);
        }
    }
}
