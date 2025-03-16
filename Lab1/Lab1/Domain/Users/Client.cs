using System.Data;
using Lab1.Domain.BankServices;
using Lab1.Domain.Repositories;

namespace Lab1.Domain.Users
{
    internal class Client(IClientRepository clientRepository) : User
    {
        public required string Surname { get; set; }
        public required string Name { get; set; }
        public required string Patronymic { get; set; }
        public  required string PassportSeriesAndNumber { get; set; }
        public required string IdNumber { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Email { get; set; }
        public bool IsApproved { get; set; } = false;
        public List<Account> Accounts { get; set; } = new List<Account>();
        public List<Credit> Credits { get; set; } = new List<Credit>();
        public List<Installment> Installments { get; set; } = new List<Installment>();
        public List<Deposit> Deposits { get; set; } = new List<Deposit>();



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

        public Credit? AddCredit(Bank bank, Period period, decimal percent, decimal amount)
        {
            if (!IsApproved)
            {
                Console.WriteLine("\nERROR!\nClient is not approved\n");
                return null;
            }

            Credit credit = new Credit()
            {
                Type = AbilityType.Credit,
                Bank = bank,
                IsApproved = false,
                Period = period,
                Persent = percent,
                Amount = amount
            };

            var creatingTask = clientRepository.CreateBankAbilityAsync(credit, CancellationToken.None);
            creatingTask.Wait();
            var clientCreditTask = clientRepository.AddClientBankAbilityRecordAsync(this, credit, CancellationToken.None);
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

            var deletingTask = clientRepository.DeleteBankAbilityAsync(credit, CancellationToken.None);
            deletingTask.Wait();
            var deletingClientCreditTask = clientRepository.RemoveClientBankAbilityRecordAsync(this, credit, CancellationToken.None);
            deletingClientCreditTask.Wait();

            Credits.Remove(credit);
        }

        public Installment? AddInstallment(Bank bank, Period period, decimal percent, decimal amount)
        {
            if (!IsApproved)
            {
                Console.WriteLine("\nERROR!\nClient is not approved\n");
                return null;
            }

            Installment installment = new Installment()
            {
                Type = AbilityType.Installment,
                Bank = bank,
                IsApproved = false,
                Period = period,
                Persent = percent,
                Amount = amount
            };

            var creatingTask = clientRepository.CreateBankAbilityAsync(installment, CancellationToken.None);
            creatingTask.Wait();
            var clientCreditTask = clientRepository.AddClientBankAbilityRecordAsync(this, installment, CancellationToken.None);
            clientCreditTask.Wait();

            Installments.Add(installment);

            return installment;
        }

        public void DeleteInstallment(Installment installment)
        {
            if (!IsApproved)
            {
                Console.WriteLine("\nERROR!\nClient is not approved\n");
                return;
            }

            var deletingTask = clientRepository.DeleteBankAbilityAsync(installment, CancellationToken.None);
            deletingTask.Wait();
            var deletingClientCreditTask = clientRepository.RemoveClientBankAbilityRecordAsync(this, installment, CancellationToken.None);
            deletingClientCreditTask.Wait();

            Installments.Remove(installment);
        }

        public Deposit? AddDeposit(Bank bank, Period period, decimal percent, decimal amount)
        {
            if (!IsApproved)
            {
                Console.WriteLine("\nERROR!\nClient is not approved\n");
                return null;
            }

            Deposit deposit = new Deposit()
            {
                Type = AbilityType.Deposit,
                Bank = bank,
                IsApproved = false,
                Period = period,
                Persent = percent,
                Amount = amount
            };

            var creatingTask = clientRepository.CreateBankAbilityAsync(deposit, CancellationToken.None);
            creatingTask.Wait();
            var clientCreditTask = clientRepository.AddClientBankAbilityRecordAsync(this, deposit, CancellationToken.None);
            clientCreditTask.Wait();

           Deposits.Add(deposit);

            return deposit;
        }

        public void DeleteDeposit(Deposit deposit)
        {
            if (!IsApproved)
            {
                Console.WriteLine("\nERROR!\nClient is not approved\n");
                return;
            }

            var deletingTask = clientRepository.DeleteBankAbilityAsync(deposit, CancellationToken.None);
            deletingTask.Wait();
            var deletingClientCreditTask = clientRepository.RemoveClientBankAbilityRecordAsync(this, deposit, CancellationToken.None);
            deletingClientCreditTask.Wait();

            Deposits.Remove(deposit);
        }
    }
}
