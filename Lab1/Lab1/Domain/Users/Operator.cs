using Lab1.Domain.BankServices;
using Lab1.Domain.Repositories;

namespace Lab1.Domain.Users
{
    internal class Operator(IOperatorRepository operatorRepository, ITransferRepository transferRepository) : User
    {
        public required string IdNumber { get; set; }
        public required string Name { get; set; }

        public void RealizeSalaryProject(Company company)
        {
            if (company.SalaryProject == null)
            {
                Console.WriteLine($"\nERROR!\nCompany {company.Name} has no salary project\n");
                return;
            }

            if (company.SalaryProject.IsCompleted == true)
            {
                Console.WriteLine("Salary project already completed!");
                return;
            }

            decimal TotalAmountOfSalaryProject = 0m;
            foreach (var sal in  company.SalaryProject.Salaries.Values)
            {
                TotalAmountOfSalaryProject += sal;
            }

            if (company.Account.Amount < TotalAmountOfSalaryProject)
            {
                Console.WriteLine($"\nERROR!\nCompany {company.Name} hasn't got enough money!\n");
                return;
            }

            if (company.Account.IsActive == false)
            {
                Console.WriteLine($"\nERROR!\nCompany account is not active!\n");
                return;
            }

            foreach (var sal in company.SalaryProject.Salaries)
            {
                CancellationToken cancellationToken = new CancellationToken();

                company.Account.Amount -= sal.Value;
                sal.Key.Amount += sal.Value;

                Transfer transfer = new Transfer()
                {
                    SenderAccount = company.Account,
                    RecipienAccount = sal.Key,
                    Amount = sal.Value
                };

                var updateSenderAccountTask = transferRepository.UpdateAccountAmountAsync(transfer.SenderAccount, cancellationToken);
                var updateRecipientAccountTask = transferRepository.UpdateAccountAmountAsync(transfer.RecipienAccount, cancellationToken);
                var createTransferTask = transferRepository.CreateAsync(transfer, cancellationToken);
                Task.WaitAll(updateSenderAccountTask, updateRecipientAccountTask, createTransferTask);
            }

            company.SalaryProject.IsCompleted = true;
            operatorRepository.CompleteSalaryProjectAsync(company.SalaryProject, CancellationToken.None).Wait();
        }

        public override string ToString()
        {
            return $"Operator [Login: {Login}, Name: {Name}, IdNumber: {IdNumber}, Password: ****]";
        }
    }
}
