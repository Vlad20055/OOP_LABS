using Lab1.Domain.Repositories;
using Lab1.Domain.Users;
using Lab1.Infrastructure.Options;
using Npgsql;

namespace Lab1.Infrastructure.Repositories
{
    internal class AdministratorRepository : IAdministratorRepository
    {
        public async Task CreateAsync(Administrator administrator, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
        INSERT INTO administrators
        (IdNumber, Name, Login, Password)
        VALUES
        (@IdNumber, @Name, @Login, @Password)
        """;

            await using var command = new NpgsqlCommand(SQLquery, connection);

            command.Parameters.AddWithValue("@IdNumber", administrator.IdNumber);
            command.Parameters.AddWithValue("@Name", administrator.Name);
            command.Parameters.AddWithValue("@Login", administrator.Login);
            command.Parameters.AddWithValue("@Password", administrator.Password);

            await command.ExecuteScalarAsync(cancellationToken);
        }

        public async Task<Administrator?> ReadAsync(string login, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
        SELECT IdNumber, Name, Login, Password
        FROM administrators
        WHERE Login = @Login
        """;

            await using var command = new NpgsqlCommand(SQLquery, connection);
            command.Parameters.AddWithValue("@Login", login);

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);

            if (await reader.ReadAsync(cancellationToken))
            {
                return new Administrator(new TransferRepository())
                {
                    IdNumber = reader.GetString(reader.GetOrdinal("IdNumber")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Login = reader.GetString(reader.GetOrdinal("Login")),
                    Password = reader.GetString(reader.GetOrdinal("Password")),
                    Role = UserRole.Administrator
                };
            }

            return null; // Return null if no administrator found with this login
        }

        public async Task DeleteAsync(Administrator administrator, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
        DELETE FROM administrators
        WHERE Login = @Login
        """;

            await using var command = new NpgsqlCommand(SQLquery, connection);
            command.Parameters.AddWithValue("@Login", administrator.Login);

            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }
}
