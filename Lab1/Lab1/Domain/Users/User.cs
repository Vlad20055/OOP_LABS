using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    internal class User
    {
        public UserRole Role { get; set; } // who is the user (Client, Manager, ...)
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
