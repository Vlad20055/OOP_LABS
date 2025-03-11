using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Domain.Users
{
    internal class Manager : User
    {
        public string IdNumber { get; set; }
        public string Name { get; set; }

        // need to add Login and Password from User to database
    }
}
