

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
        public int Id { get; set; }
        public required CompanyType Type { get; set; }
        public required string Name { get; set; }
        public required int PayerAccountNumber { get; set; } // AccountId - shows company Account
        public required int BankIdentificationCode { get; set; } // BankId - shows what Bank Company is related to
        public required string Address { get; set; }

    }
}
