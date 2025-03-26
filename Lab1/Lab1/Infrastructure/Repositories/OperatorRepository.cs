using Lab1.Domain;
using Lab1.Domain.BankServices;
using Lab1.Domain.Repositories;
using Lab1.Domain.Users;
using Lab1.Infrastructure.Options;
using Npgsql;

namespace Lab1.Infrastructure.Repositories
{
    internal class OperatorRepository : IOperatorRepository
    {
        public async Task CreateAsync(Operator @operator, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
        INSERT INTO operators
        (IdNumber, Name, Login, Password)
        VALUES
        (@IdNumber, @Name, @Login, @Password)
        """;

            await using var command = new NpgsqlCommand(SQLquery, connection);

            command.Parameters.AddWithValue("@IdNumber", @operator.IdNumber);
            command.Parameters.AddWithValue("@Name", @operator.Name);
            command.Parameters.AddWithValue("@Login", @operator.Login);
            command.Parameters.AddWithValue("@Password", @operator.Password);

            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        public async Task<Operator?> ReadAsync(string login, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
        SELECT IdNumber, Name, Login, Password
        FROM operators
        WHERE Login = @Login
        """;

            await using var command = new NpgsqlCommand(SQLquery, connection);
            command.Parameters.AddWithValue("@Login", login);

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);

            if (await reader.ReadAsync(cancellationToken))
            {
                return new Operator(new OperatorRepository(), new TransferRepository())
                {
                    IdNumber = reader.GetString(reader.GetOrdinal("IdNumber")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Login = reader.GetString(reader.GetOrdinal("Login")),
                    Password = reader.GetString(reader.GetOrdinal("Password"))
                };
            }

            return null; // Return null if no operator found with this login
        }

        public async Task DeleteAsync(Operator @operator, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
        DELETE FROM operators
        WHERE Login = @Login
        """;

            await using var command = new NpgsqlCommand(SQLquery, connection);
            command.Parameters.AddWithValue("@Login", @operator.Login);

            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        public async Task CompleteSalaryProjectAsync(SalaryProject salaryProject, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string sql = """
        UPDATE salary_projects
        SET IsCompleted = true
        WHERE IdNumber = @IdNumber
        """;

            await using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@IdNumber", salaryProject.IdNumber);

            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }
}
