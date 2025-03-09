using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Domain.Users
{
    internal class CompanySpecialist
    {
        public Bank Bank { get; set; }
        public ICollection<Account> Accounts { get; set; }
        public Company Company { get; set; } // shows what Company CompanySpecialist is related to
    }
}
