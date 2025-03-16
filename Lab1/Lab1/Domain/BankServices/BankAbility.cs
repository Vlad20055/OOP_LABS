

namespace Lab1.Domain.BankServices
{
    public enum AbilityType
    {
        Credit,
        Installment,
        Deposit
    }

    public enum Period
    {
        Month_3 = 3,
        Month_6 = 6,
        Month_12 = 12,
        Month_24 = 24,
        Long_period = 0
    }

    internal class BankAbility
    {
        public AbilityType Type { get; set; } // we don't save this property in database
        public int IdNumber { get; set; }
        public Bank? Bank { get; set; }
        public bool IsApproved { get; set; }
        public Period Period { get; set; }
        public decimal Persent { get; set; }
        public decimal Amount { get; set; }
    }
}
