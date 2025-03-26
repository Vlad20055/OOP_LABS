﻿

namespace Lab1.Domain.BankServices
{
    internal class Transfer
    {
        public int IdNumber { get; set; } // generated by database
        public required Account SenderAccount { get; set; }
        public required Account RecipienAccount { get; set; }
        public required decimal Amount { get; set; }
        public bool IsCancelled { get; set; } = false;

        public override string ToString()
        {
            return $"Transfer [ID: {IdNumber}, " +
                   $"From: {SenderAccount.IdNumber}, " +
                   $"To: {RecipienAccount.IdNumber}, " +
                   $"Amount: {Amount:C}, " +
                   $"  Status: {(IsCancelled ? "❌ Cancelled" : "✅ Completed")}";
        }
    }
}
