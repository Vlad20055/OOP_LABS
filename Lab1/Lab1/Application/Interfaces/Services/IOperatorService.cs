

using Lab1.Domain.Users;

namespace Lab1.Application.Interfaces.Services
{
    internal interface IOperatorService
    {
        public Operator AddOperator(string login, string password, string idNumber, string name);
        public void DeleteOperator(Operator @operator);
    }
}
