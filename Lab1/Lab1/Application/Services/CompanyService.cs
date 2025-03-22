

using Lab1.Application.Interfaces.Services;
using Lab1.Domain;
using Lab1.Domain.Repositories;
using Lab1.Infrastructure.Repositories;

namespace Lab1.Application.Services
{
    internal class CompanyService(ICompanyRepository companyRepository) : ICompanyService
    {
        public Company Register(string name, CompanyType companyType, string address, Bank bank)
        {
            var readingTask = companyRepository.ReadAsync(name, CancellationToken.None);
            Task.WaitAny(readingTask);
            Company? company = readingTask.Result;

            if (company != null)
            {
                Console.WriteLine("\nREGISTRATION ERROR\nCompany already exists\n");
                return company;
            }

            company = new Company()
            {
                Name = name,
                CompanyType = companyType,
                Address = address,
                Account = new Account()
                {
                    Amount = 1000000000,
                    Bank = bank,
                    IsActive = true
                }
            };

            var creationAccountTask = companyRepository.CreateAccountAsync(company.Account, CancellationToken.None);
            creationAccountTask.Wait();
            var creationAccountBankTask = companyRepository.AddAccountBankRecordAsync(company.Account, bank, CancellationToken.None);
            creationAccountBankTask.Wait();
            var creationTask = companyRepository.CreateAsync(company, CancellationToken.None);
            creationTask.Wait();
            return company;
        }

        public void Unregister(Company company) // Doesn't work correctly!
        {
            var deletingAccountTask = companyRepository.DeleteAccountAsync(company.Account, CancellationToken.None);
            deletingAccountTask.Wait();
            var deletingAccountBankTask = companyRepository.RemoveAccountBankRecordAsync(company.Account, company.Account.Bank, CancellationToken.None);
            deletingAccountBankTask.Wait();
            var deletingTask = companyRepository.DeleteAsync(company, CancellationToken.None);
            Task.WaitAny(deletingTask);
        }
    }
}
