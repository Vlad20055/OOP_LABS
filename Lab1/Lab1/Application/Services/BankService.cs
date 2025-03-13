using Lab1.Application.Interfaces.Services;
using Lab1.Domain;
using Lab1.Domain.Repositories;

namespace Lab1.Application.Services
{
    internal class BankService(IBankRepository bankRepository) : IBankService
    {
        public Bank Register(string name)
        {
            var readingTask = bankRepository.ReadAsync(name, CancellationToken.None);
            Task.WaitAny(readingTask);
            Bank? bank = readingTask.Result;

            if (bank != null) 
            {
                Console.WriteLine("\nREGISTRATION ERROR\nBank already exists\n");
                return bank;
            }

            bank = new Bank()
            {
                Name = name
            };

            var creationTask = bankRepository.CreateAsync(bank, CancellationToken.None);
            Task.WaitAny(creationTask);
            return bank;
        }

        public void Unregister(Bank bank)
        {
            var deletingTask = bankRepository.DeleteAsync(bank, CancellationToken.None);
            Task.WaitAny(deletingTask);
        }
    }
}
