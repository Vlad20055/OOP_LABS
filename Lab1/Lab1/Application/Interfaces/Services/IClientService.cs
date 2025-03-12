using Lab1.Domain.Users;

namespace Lab1.Application.Interfaces.Services
{
    internal interface IClientService
    {
        public Client Register(
            string surname,
            string name,
            string patronymic,
            string passportSeriesAndNumber,
            string idNumber,
            string phoneNumber,
            string email,
            string login,
            string password
            );

        public void DeleteClient(Client client);
    }
}
