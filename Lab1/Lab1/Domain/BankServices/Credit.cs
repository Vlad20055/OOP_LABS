﻿namespace Lab1.Domain.BankServices
{
    internal class Credit : BankAbility
    {
    
        public override string ToString()
        {
            return $"Credit ID: {IdNumber}, " +
                   $"Bank: {Bank?.Name ?? "N/A"} (ID: {Bank?.Id}), " +
                   $"IsApproved: {IsApproved}, " +
                   $"Period: {Period.ToString() ?? "N/A"}, " +
                   $"Interest Rate: {Persent}%, " +
                   $"Remaining Amount: {Amount:C}";
        }
    }
}
