

namespace Lab1.Domain.BankServices
{
    internal class SalaryProject
    {
        public int IdNumber { get; set; }
        public int BankId { get; set; }
        public required string CompanyName {  get; set; }
        public bool IsCompleted { get; set; } = false;
        public Dictionary<Account, decimal> Salaries { get; set; } = new Dictionary<Account, decimal>();
        
    }
}
