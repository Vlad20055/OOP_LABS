using Lab1.Domain.Users;

namespace Lab1.Application.Interfaces.Services
{
    internal interface IManagerService
    {
        public Manager AddManager(
            string idNumber,
            string name,
            string login,
            string password
            );

        public void DeleteManager(Manager manager);
    }
}
