

using Lab1.Application.Interfaces.Services;
using Lab1.Domain.Repositories;
using Lab1.Domain.Users;

namespace Lab1.Application.Services
{
    internal class OperatorService(IOperatorRepository operatorRepository, ITransferRepository transferRepository) : IOperatorService
    {
        public Operator AddOperator(
            string login,
            string password,
            string idNumber,
            string name
            )
        {
            var readingTask = operatorRepository.ReadAsync(login, CancellationToken.None);
            readingTask.Wait();
            Operator? @operator = readingTask.Result;

            if (@operator != null)
            {
                Console.WriteLine("\nERROR!\nOpeerator already exists\n");
                return @operator;
            }

            @operator = new Operator(operatorRepository, transferRepository)
            {
                Login = login,
                Password = password,
                IdNumber = idNumber,
                Name = name,
                Role = UserRole.Operator,
            };

            operatorRepository.CreateAsync(@operator, CancellationToken.None).Wait();
            return @operator;
        }

        public void DeleteOperator(Operator @operator)
        {
            operatorRepository.DeleteAsync(@operator, CancellationToken.None).Wait();
        }
    }
}
