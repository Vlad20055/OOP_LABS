using Lab1.Domain.BankServices;
using Lab1.Domain.Repositories;

namespace Lab1.Domain.Users
{
    internal class Manager(IManagerRepository managerRepository) : User
    {
        public required string IdNumber { get; set; }
        public required string Name { get; set; }

        public async Task ApproveClient(Client client)
        {
            await managerRepository.ApproveClientAsync(client, CancellationToken.None);
            client.IsApproved = true;
        }

        public async Task ApproveCredit(Credit credit)
        {
            await managerRepository.ApproveClientBankAbilityAsync(credit, CancellationToken.None);
            credit.IsApproved = true;
        }

        public async Task ApproveInstallment(Installment installment)
        {
            await managerRepository.ApproveClientBankAbilityAsync(installment, CancellationToken.None);
            installment.IsApproved = true;
        }

        public async Task ApproveDeposit(Deposit deposit)
        {
            await managerRepository.ApproveClientBankAbilityAsync(deposit, CancellationToken.None);
            deposit.IsApproved = true;
        }
    }
}
