using Lab1.Domain.BankServices;

namespace Lab1.Domain.Repositories
{
    internal interface ITransferRepository
    {
        public Task CreateAsync(Transfer transfer, CancellationToken cancellationToken);
        public Task<List<Transfer>> ReadBySenderAccountId(int senderAccountId,  CancellationToken cancellationToken);
        public Task<List<Transfer>> ReadAllTransfersAsync(CancellationToken cancellationToken);
        public Task UpdateAccountAmountAsync(Account account, CancellationToken cancellationToken);
        public Task CancelTransferAsync(int idNumber, CancellationToken cancellationToken);
    }
}
