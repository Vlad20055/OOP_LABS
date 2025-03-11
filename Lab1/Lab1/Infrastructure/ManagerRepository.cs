using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Npgsql;
using Lab1.Infrastructure.Options;
using Lab1.Application.Interfaces.Repositories;
using Lab1.Domain.Users;

namespace Lab1.Infrastructure
{
    internal class ManagerRepository : IManagerRepository
    {
        public async Task<int> CreateAsync(Manager manager, CancellationToken cancellationToken) 
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
                INSERT INTO managers
                (IdNumber, Name)
                VALUES
                (@IdNumber, @Name)
                """;


            var command = new NpgsqlCommand(SQLquery, connection);
            
            command.Parameters.AddWithValue("@IdNumber", manager.IdNumber);
            command.Parameters.AddWithValue("@Name", manager.Name);
            
            return (int)(await command.ExecuteScalarAsync(cancellationToken) ?? throw new NpgsqlException());
        }

        public async Task DeleteAsync(Manager manager, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
                DELETE FROM managers
                WHERE IdNumber = @IdNumber
                """;

            var command = new NpgsqlCommand(SQLquery, connection);

            command.Parameters.AddWithValue("@IdNumber", manager.IdNumber);

            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        public async Task UpdateClientAsync(Client client, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
                UPDATE clients
                SET IsApproved = @IsApproved
                WHERE IdNumber = @IdNumber;
                """;

            var command = new NpgsqlCommand(SQLquery, connection);

            command.Parameters.AddWithValue("@IsApproved", true);
            command.Parameters.AddWithValue("@IdNumber", client.IdNumber);

            await command.ExecuteNonQueryAsync();
        }
    }
}
