

namespace Lab1.Domain.Users
{
    internal class Client : User
    {
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string PassportSeriesAndNumber { get; set; }
        public string IdNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public ICollection<Bank> Banks { get; set; }
        public ICollection<Account> Accounts { get; set; }

    }
}
