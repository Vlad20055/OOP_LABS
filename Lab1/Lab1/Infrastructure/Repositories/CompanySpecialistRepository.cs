using Lab1.Domain.BankServices;
using Lab1.Domain.Repositories;
using Lab1.Infrastructure.Options;
using Npgsql;

namespace Lab1.Infrastructure.Repositories
{
    internal class CompanySpecialistRepository : ICompanySpecialistRepository
    {
        public async Task CreateSalaryProjectAsync(SalaryProject salaryProject, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
                INSERT INTO salary_projects (BankId, CompanyName, IsCompleted)
                VALUES (@BankId, @CompanyName, @IsCompleted)
                RETURNING IdNumber;
                """;

            await using var command = new NpgsqlCommand(SQLquery, connection);

            command.Parameters.AddWithValue("@BankId", salaryProject.BankId);
            command.Parameters.AddWithValue("@CompanyName", salaryProject.CompanyName);
            command.Parameters.AddWithValue("@IsCompleted", salaryProject.IsCompleted);

            var result = await command.ExecuteScalarAsync(cancellationToken);

            if (result == null)
            {
                throw new NpgsqlException("Failed to create salary project. No ID was returned.");
            }

            salaryProject.IdNumber = (int)result;
        }

        public async Task DeleteSalaryProjectAsync(SalaryProject salaryProject, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
                DELETE FROM salary_projects
                WHERE IdNumber = @IdNumber;
                """;

            await using var command = new NpgsqlCommand(SQLquery, connection);

            command.Parameters.AddWithValue("@IdNumber", salaryProject.IdNumber);

            await command.ExecuteNonQueryAsync(cancellationToken);
        }

    }
}
