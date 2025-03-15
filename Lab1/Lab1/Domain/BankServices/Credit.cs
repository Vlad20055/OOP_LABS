namespace Lab1.Domain.BankServices
{
    public enum CreditPeriod
    {
        Month_3 = 3,
        Month_6 = 6,
        Month_12 = 12,
        Month_24 = 24,
        Long_period = 0
    }

    internal class Credit
    {
        public int IdNumber { get; set; }
        public Bank? Bank { get; set; }
        public bool IsApproved { get; set; }
        public CreditPeriod? Period { get; set; }
        public decimal Persent { get; set; }
        public decimal Rest { get; set; }


        public override string ToString()
        {
            return $"Credit ID: {IdNumber}, " +
                   $"Bank: {Bank?.Name ?? "N/A"} (ID: {Bank?.Id}), " +
                   $"IsApproved: {IsApproved}, " +
                   $"Period: {Period?.ToString() ?? "N/A"}, " +
                   $"Interest Rate: {Persent}%, " +
                   $"Remaining Amount: {Rest:C}";
        }
    }
}
