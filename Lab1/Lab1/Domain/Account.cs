using Lab1.Domain.Users;

namespace Lab1.Domain
{
    internal class Account()
    {
        public int? IdNumber { get; set; }
        public Client? Client { get; set; }
        public Bank? Bank { get; set; }
        public decimal Amount { get; set; }
        public bool IsActive { get; set; }
    }
}
