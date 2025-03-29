

namespace Lab1.Domain.Users
{
    public enum UserRole
    {
        Administrator,
        Clilent,
        CompanySpecialist,
        Manager,
        Operator
    }

    internal abstract class User
    {
        public UserRole Role { get; set; } // who is the user (Client, Manager, ...)
        public required string Login { get; set; }
        public required string Password { get; set; }
    }
}
