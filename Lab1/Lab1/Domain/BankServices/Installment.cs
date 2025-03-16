

namespace Lab1.Domain.BankServices
{
    internal class Installment : BankAbility
    {
        public override string ToString()
        {
            return $"Installment ID: {IdNumber}, " +
                   $"Bank: {Bank?.Name ?? "N/A"} (ID: {Bank?.Id}), " +
                   $"IsApproved: {IsApproved}, " +
                   $"Period: {Period.ToString() ?? "N/A"}, " +
                   $"Interest Rate: {Persent}%, " +
                   $"Remaining Amount: {Amount:C}";
        }
    }
}
