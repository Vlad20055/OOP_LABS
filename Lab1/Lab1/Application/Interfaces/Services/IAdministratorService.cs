using Lab1.Domain.Users;

namespace Lab1.Application.Interfaces.Services
{
    internal interface IAdministratorService
    {
        public Administrator AddAdministrator(string login, string password, string idNumber, string name);
        public void DeleteAdministrator(Administrator administrator);
    }
}
