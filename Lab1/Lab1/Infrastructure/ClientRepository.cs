using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lab1.Application.Interfaces.Repositories;
using Lab1.Domain.Users;
using Lab1.Infrastructure.Options;
using Npgsql;


namespace Lab1.Infrastructure
{
    internal class ClientRepository : IClientRepository
    {
        public async Task<int> CreateAsync(Client client, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
                INSERT INTO clients
                (Surname, Name, Patronymic, PassportSeriesAndNumber, IdNumber, PhoneNumber, Email, IsApproved)
                VALUES
                (@Surname, @Name, @Patronymic, @PassportSeriesAndNumber, @IdNumber, @PhoneNumber, @Email, @IsApproved)
                """;

            var command = new NpgsqlCommand(SQLquery, connection);
            command.Parameters.AddWithValue("@Surname", client.Surname);
            command.Parameters.AddWithValue("@Name", client.Name);
            command.Parameters.AddWithValue("@Patronymic", client.Patronymic);
            command.Parameters.AddWithValue("@PassportSeriesAndNumber", client.PassportSeriesAndNumber);
            command.Parameters.AddWithValue("@IdNumber", client.IdNumber);
            command.Parameters.AddWithValue("@PhoneNumber", client.PhoneNumber);
            command.Parameters.AddWithValue("@Email", client.Email);
            command.Parameters.AddWithValue("@IsApproved", false);

            // add all his accounts and banks

            return (int)(await command.ExecuteScalarAsync(cancellationToken) ?? throw new NpgsqlException());
        }
    }
}
