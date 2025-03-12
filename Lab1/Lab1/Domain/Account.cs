using Lab1.Domain.Repositories;

namespace Lab1.Domain
{
    internal class Account()
    {
        public int? IdNumber { get; set; }
        public Bank Bank { get; set; }
        public decimal Amount { get; set; }
        public bool IsActive { get; set; }
    }
}
