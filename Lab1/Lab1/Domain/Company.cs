using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Domain
{
    public enum CompanyType
    {
        IndividualEntrepreneur,
        LimitedLiabilityCompany,
        ClosedJointStockCompany
    }

    internal class Company
    {
        public CompanyType Type { get; set; }
        public string Name { get; set; }
        public string PayerAccountNumber { get; set; }
        public string BankIdentificationCode { get; set; } // shows what Bank Company is related to
        public string Address { get; set; }
    }
}
