using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab1.Domain;

namespace Lab1.Application.Interfaces.Services
{
    internal interface IBankService
    {
        public Bank Register(string name);
        public void Unregister(Bank bank);
    }
}
