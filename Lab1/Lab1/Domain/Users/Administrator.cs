using Lab1.Domain.BankServices;
using Lab1.Domain.Repositories;

namespace Lab1.Domain.Users
{
    internal class Administrator(ITransferRepository transferRepository) : User
    {
        public required string IdNumber { get; set; }
        public required string Name { get; set; }

        public List<Transfer> GetTransfers(Account senderAccount)
        {
            var readingTask = transferRepository.ReadBySenderAccountId(senderAccount.IdNumber, CancellationToken.None);
            readingTask.Wait();

            return readingTask.Result;
        }

        public List<Transfer> GetAllTransfers()
        {
            var readingTask = transferRepository.ReadAllTransfersAsync(CancellationToken.None);
            readingTask.Wait();

            return readingTask.Result;
        }

        public void CancelTransfer(Transfer transfer)
        {
            if (transfer.IsCancelled == true)
            {
                Console.WriteLine("\nERROR\nTransfer already cancelled!\n");
                return;
            }

            transfer.IsCancelled = true;
            transfer.RecipienAccount.Amount -= transfer.Amount;
            transfer.SenderAccount.Amount += transfer.Amount;

            CancellationToken cancellationToken = CancellationToken.None;
            transferRepository.CancelTransferAsync(transfer.IdNumber, cancellationToken).Wait();
            transferRepository.UpdateAccountAmountAsync(transfer.SenderAccount, cancellationToken).Wait();
            transferRepository.UpdateAccountAmountAsync(transfer.RecipienAccount, cancellationToken).Wait();
        }

        public override string ToString()
        {
            return $"Administrator [ID: {IdNumber}, Name: {Name}, Login: {Login}, Password: ****]";
        }
    }
}
