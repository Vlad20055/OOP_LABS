using Lab1.Domain;
using Lab1.Domain.BankServices;
using Lab1.Domain.Repositories;
using Lab1.Infrastructure.Options;
using Npgsql;

namespace Lab1.Infrastructure.Repositories
{
    internal class TransferRepository : ITransferRepository
    {
        public async Task CreateAsync(Transfer transfer, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
        INSERT INTO transferes
        (SenderAccountId, RecipienAccountId, Amount, IsCancelled)
        VALUES
        (@SenderAccountId, @RecipienAccountId, @Amount, @IsCancelled)
        RETURNING IdNumber
        """;

            await using var command = new NpgsqlCommand(SQLquery, connection);

            command.Parameters.AddWithValue("@SenderAccountId", transfer.SenderAccount.IdNumber);
            command.Parameters.AddWithValue("@RecipienAccountId", transfer.RecipienAccount.IdNumber);
            command.Parameters.AddWithValue("@Amount", transfer.Amount);
            command.Parameters.AddWithValue("@IsCancelled", transfer.IsCancelled);

            // Execute and get the generated IdNumber
            var result = await command.ExecuteScalarAsync(cancellationToken);
            transfer.IdNumber = (int)(result ?? throw new Exception("Failed to get transfer ID"));
        }
        public async Task<Account?> ReadAccountAsync(int idNumber, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
        SELECT a.IdNumber, a.Amount, a.IsActive, b.Id AS BankId, b.Name AS BankName
        FROM accounts a
        INNER JOIN account_bank_records abr ON a.IdNumber = abr.IdNumber
        INNER JOIN banks b ON abr.Name = b.Name
        WHERE a.IdNumber = @IdNumber
        """;

            await using var command = new NpgsqlCommand(SQLquery, connection);
            command.Parameters.AddWithValue("@IdNumber", idNumber);

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);

            if (await reader.ReadAsync(cancellationToken))
            {
                var account = new Account
                {
                    IdNumber = reader.GetInt32(reader.GetOrdinal("IdNumber")),
                    Amount = reader.GetDecimal(reader.GetOrdinal("Amount")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                    Bank = new Bank
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("BankId")),
                        Name = reader.GetString(reader.GetOrdinal("BankName"))
                    }
                };

                return account;
            }

            return null; // Return null if no matching account is found
        }
        public async Task<List<Transfer>> ReadBySenderAccountId(int senderAccountId, CancellationToken cancellationToken)
        {
            var transfers = new List<Transfer>();

            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
        SELECT IdNumber, RecipienAccountId, Amount, IsCancelled
        FROM transferes
        WHERE SenderAccountId = @senderAccountId
        """;

            await using var command = new NpgsqlCommand(SQLquery, connection);
            command.Parameters.AddWithValue("@senderAccountId", senderAccountId);

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);

            while (await reader.ReadAsync(cancellationToken))
            {
                var recipientAccountId = reader.GetInt32(reader.GetOrdinal("RecipienAccountId"));
                var recipientAccount = await ReadAccountAsync(recipientAccountId, cancellationToken);
                var senderAccount = await ReadAccountAsync(senderAccountId, cancellationToken);

                if (recipientAccount != null && senderAccount != null)
                {
                    transfers.Add(new Transfer
                    {
                        IdNumber = reader.GetInt32(reader.GetOrdinal("IdNumber")),
                        Amount = reader.GetDecimal(reader.GetOrdinal("Amount")),
                        IsCancelled = reader.GetBoolean(reader.GetOrdinal("IsCancelled")),
                        SenderAccount = senderAccount,
                        RecipienAccount = recipientAccount
                    });
                }
            }

            return transfers;
        }
        public async Task UpdateAccountAmountAsync(Account account, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
        UPDATE accounts
        SET Amount = @Amount
        WHERE IdNumber = @IdNumber
        """;

            await using var command = new NpgsqlCommand(SQLquery, connection);
            command.Parameters.AddWithValue("@Amount", account.Amount);
            command.Parameters.AddWithValue("@IdNumber", account.IdNumber);

            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }
}
