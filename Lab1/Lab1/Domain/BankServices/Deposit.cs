

namespace Lab1.Domain.BankServices
{
    internal class Deposit : BankAbility
    {
        public override string ToString()
        {
            return $"Deposit ID: {IdNumber}, " +
                   $"Bank: {Bank?.Name ?? "N/A"} (ID: {Bank?.Id}), " +
                   $"IsApproved: {IsApproved}, " +
                   $"Period: {Period.ToString() ?? "N/A"}, " +
                   $"Interest Rate: {Persent}%, " +
                   $"Amount: {Amount:C}";

        }
    }
}
