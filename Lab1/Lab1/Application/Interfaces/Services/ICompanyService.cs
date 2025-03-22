

using Lab1.Domain;
using Lab1.Domain.Repositories;
using Lab1.Infrastructure.Repositories;

namespace Lab1.Application.Interfaces.Services
{
    internal interface ICompanyService
    {
        public Company Register(string name, CompanyType companyType, string address, Bank bank);
        public void Unregister(Company company);
    }
}
