﻿using Lab1.Domain.Users;

namespace Lab1.Domain
{
    internal class Account()
    {
        public int? IdNumber { get; set; }
        public Bank? Bank { get; set; }
        public decimal Amount { get; set; }
        public bool IsActive { get; set; }

        public override string ToString()
        {
            return $"Account ID: {IdNumber?.ToString() ?? "N/A"}, " +
                   $"Bank: {Bank?.Name ?? "N/A"} (ID: {Bank?.Id}), " +
                   $"Amount: {Amount:C}, " +
                   $"IsActive: {IsActive}";
        }
    }
}
