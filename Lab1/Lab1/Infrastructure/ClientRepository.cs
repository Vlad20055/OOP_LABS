using Lab1.Domain.Repositories;
using Lab1.Domain.Users;
using Lab1.Infrastructure.Options;
using Npgsql;


namespace Lab1.Infrastructure
{
    internal class ClientRepository : IClientRepository
    {
        public async Task CreateAsync(Client client, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
                INSERT INTO clients
                (Surname, Name, Patronymic, PassportSeriesAndNumber, IdNumber, PhoneNumber, Email, IsApproved, Login, Password)
                VALUES
                (@Surname, @Name, @Patronymic, @PassportSeriesAndNumber, @IdNumber, @PhoneNumber, @Email, @IsApproved, @Login, @Password)
                RETURNING Login
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
            command.Parameters.AddWithValue("@Login", client.Login);
            command.Parameters.AddWithValue("@Password", client.Password);

            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        public async Task DeleteAsync(Client client, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
                DELETE FROM clients
                WHERE Login = @Login
                """;

            var command = new NpgsqlCommand(SQLquery, connection);

            command.Parameters.AddWithValue("@Login", client.Login);

            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        public async Task<Client> ReadAsync(string login, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
                SELECT * FROM clients
                WHERE Login = @Login
                """;

            var command = new NpgsqlCommand(SQLquery, connection);

            command.Parameters.AddWithValue("@Login", login);

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);

            var client = new Client();

            if (!await reader.ReadAsync(cancellationToken)) throw new NpgsqlException();

            client.Surname = (string)reader["Surname"];
            client.Name = (string)reader["Name"];
            client.Patronymic = (string)reader["Patronymic"];
            client.PassportSeriesAndNumber = (string)reader["PassportSeriesAndNumber"];
            client.IdNumber = (string)reader["IdNumber"];
            client.PhoneNumber = (string)reader["PhoneNumber"];
            client.Email = (string)reader["Email"];
            client.Login = (string)reader["Login"];
            client.Password = (string)reader["Password"];
            client.Role = UserRole.Clilent;

            return client;
        }
    }
}
