﻿

namespace Lab1.Domain.BankServices
{
    internal class SalaryProjectRequest
    {
        public int IdNumber {  get; set; } // generated by database
        public required string Login {  get; set; }
        public required string CompanyName { get; set; }
        public required Account Account { get; set; }
        public required decimal Salary {  get; set; }
        public bool IsApproved { get; set; } = false;
    }
}
