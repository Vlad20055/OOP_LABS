using System.Xml.Linq;
using Lab1.Domain;
using Lab1.Domain.Repositories;
using Lab1.Infrastructure.Options;
using Npgsql;

namespace Lab1.Infrastructure.Repositories
{
    internal class BankRepository : IBankRepository
    {
        public async Task CreateAsync(Bank bank, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
                INSERT INTO banks
                (Name)
                VALUES
                (@Name)
                RETURNING Id
                """;

            var command = new NpgsqlCommand(SQLquery, connection);

            command.Parameters.AddWithValue("@Name", bank.Name);

            var id = await command.ExecuteScalarAsync(cancellationToken);

            if (id == null) throw new NpgsqlException();

            bank.Id = (int)id;
        }

        public async Task<Bank?> ReadAsync(string name, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
                SELECT Id, Name
                FROM banks
                WHERE Name = @Name;
                """;

            await using var command = new NpgsqlCommand(SQLquery, connection);
            command.Parameters.AddWithValue("@Name", name);

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);

            if (await reader.ReadAsync(cancellationToken))
            {
                return new Bank
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name"))
                };
            }

            return null; // Return null if no bank is found
        }

        public async Task<Bank> ReadAsync(int id, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
                SELECT Id, Name
                FROM banks
                WHERE Id = @Id;
                """;

            await using var command = new NpgsqlCommand(SQLquery, connection);
            command.Parameters.AddWithValue("@Id", id);

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);

            if (await reader.ReadAsync(cancellationToken))
            {
                return new Bank
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name"))
                };
            }

            throw new NotImplementedException();
        }

        public async Task DeleteAsync(Bank bank, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
                DELETE FROM banks
                WHERE Id = @Id;
                """;

            await using var command = new NpgsqlCommand(SQLquery, connection);
            command.Parameters.AddWithValue("@Id", bank.Id);

            await command.ExecuteNonQueryAsync(cancellationToken);
        }

    }
}
