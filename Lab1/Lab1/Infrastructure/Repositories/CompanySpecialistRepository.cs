using Lab1.Domain;
using Lab1.Domain.BankServices;
using Lab1.Domain.Repositories;
using Lab1.Domain.Users;
using Lab1.Infrastructure.Options;
using Npgsql;

namespace Lab1.Infrastructure.Repositories
{
    internal class CompanySpecialistRepository : ICompanySpecialistRepository
    {

        public async Task CreateAsync(CompanySpecialist companySpecialist, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
                INSERT INTO company_specialists
                (IdNumber, Name, Login, Password, CompanyName)
                VALUES
                (@IdNumber, @Name, @Login, @Password, @CompanyName)
                RETURNING Login
                """;

            var command = new NpgsqlCommand(SQLquery, connection);

            command.Parameters.AddWithValue("@IdNumber", companySpecialist.IdNumber);
            command.Parameters.AddWithValue("@Name", companySpecialist.Name);
            command.Parameters.AddWithValue("@Login", companySpecialist.Login);
            command.Parameters.AddWithValue("@Password", companySpecialist.Password);
            command.Parameters.AddWithValue("@CompanyName", companySpecialist.Company.Name); // Assuming Company has a Name property

            await command.ExecuteNonQueryAsync(cancellationToken);
        }
        public async Task<CompanySpecialist?> ReadAsync(string login, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
        SELECT IdNumber, Name, Login, Password, CompanyName
        FROM company_specialists
        WHERE Login = @Login
        """;

            await using var command = new NpgsqlCommand(SQLquery, connection);
            command.Parameters.AddWithValue("@Login", login);

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);

            if (await reader.ReadAsync(cancellationToken))
            {
                var companySpecialist = new CompanySpecialist(new CompanySpecialistRepository())
                {
                    IdNumber = reader.GetString(reader.GetOrdinal("IdNumber")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Login = reader.GetString(reader.GetOrdinal("Login")),
                    Password = reader.GetString(reader.GetOrdinal("Password")),
                    Company = await ReadCompanyAsync(reader.GetString(reader.GetOrdinal("CompanyName")), cancellationToken) ?? throw new ArgumentNullException("No Company for CompanySpecialist")
                };

                return companySpecialist;
            }

            return null; // Return null if no matching record is found
        }
        public async Task DeleteAsync(CompanySpecialist companySpecialist, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
        DELETE FROM company_specialists
        WHERE Login = @Login
        """;

            await using var command = new NpgsqlCommand(SQLquery, connection);
            command.Parameters.AddWithValue("@Login", companySpecialist.Login);

            await command.ExecuteNonQueryAsync(cancellationToken);
        }
        
        
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
        public async Task<Company?> ReadCompanyAsync(string name, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
        SELECT Id, CompanyType, Name, PayerAccountNumber, BankIdentificationCode, Address, SalaryProjectId
        FROM companies
        WHERE Name = @Name
        """;

            await using var command = new NpgsqlCommand(SQLquery, connection);
            command.Parameters.AddWithValue("@Name", name);

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);

            if (await reader.ReadAsync(cancellationToken))
            {
                var company = new Company
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    CompanyType = (CompanyType)reader.GetInt32(reader.GetOrdinal("CompanyType")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Address = reader.GetString(reader.GetOrdinal("Address")),
                    SalaryProject = null, // Initialize as null, will be set conditionally
                    Account = new Account
                    {
                        Bank = new Bank
                        {
                            Name = ""
                        }
                    }
                };

                // Fetch the Account using the ReadAccountAsync method
                int payerAccountNumber = reader.GetInt32(reader.GetOrdinal("PayerAccountNumber"));
                company.Account = await ReadAccountAsync(payerAccountNumber, cancellationToken) ?? throw new Exception("No Account for Company");

                // Check if SalaryProjectId is not null
                if (!reader.IsDBNull(reader.GetOrdinal("SalaryProjectId")))
                {
                    int salaryProjectId = reader.GetInt32(reader.GetOrdinal("SalaryProjectId"));
                    company.SalaryProject = await ReadSalaryProject(salaryProjectId, cancellationToken);
                }

                return company;
            }

            return null; // Return null if no matching company is found
        }
        public async Task UpdateCompanyAsync(int companyId, int salaryProjectId, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
        UPDATE companies
        SET SalaryProjectId = @SalaryProjectId
        WHERE Id = @CompanyId
        """;

            await using var command = new NpgsqlCommand(SQLquery, connection);
            command.Parameters.AddWithValue("@SalaryProjectId", salaryProjectId);
            command.Parameters.AddWithValue("@CompanyId", companyId);

            await command.ExecuteNonQueryAsync(cancellationToken);
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
        public async Task<Dictionary<Account, decimal>> ReadSalariesAsync(int salaryProjectId, CancellationToken cancellationToken)
        {
            var salariesDictionary = new Dictionary<Account, decimal>();

            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
        SELECT Salary, AccountId
        FROM salaries
        WHERE SalaryProjectId = @SalaryProjectId
        """;

            await using var command = new NpgsqlCommand(SQLquery, connection);
            command.Parameters.AddWithValue("@SalaryProjectId", salaryProjectId);

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);

            while (await reader.ReadAsync(cancellationToken))
            {
                decimal salary = reader.GetDecimal(reader.GetOrdinal("Salary"));
                int accountId = reader.GetInt32(reader.GetOrdinal("AccountId"));

                // Fetch the Account using the ReadAccountAsync method
                Account account = await ReadAccountAsync(accountId, cancellationToken) ?? throw new Exception("No Account for AccountId");

                // Add the Account and Salary to the dictionary
                salariesDictionary[account] = salary;
            }

            return salariesDictionary;
        }
        public async Task<SalaryProject?> ReadSalaryProject(int idNumber, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
        SELECT IdNumber, BankId, CompanyName, IsCompleted
        FROM salary_projects
        WHERE IdNumber = @IdNumber
        """;

            await using var command = new NpgsqlCommand(SQLquery, connection);
            command.Parameters.AddWithValue("@IdNumber", idNumber);

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);

            if (await reader.ReadAsync(cancellationToken))
            {
                var salaryProject = new SalaryProject
                {
                    IdNumber = reader.GetInt32(reader.GetOrdinal("IdNumber")),
                    BankId = reader.GetInt32(reader.GetOrdinal("BankId")),
                    CompanyName = reader.GetString(reader.GetOrdinal("CompanyName")),
                    IsCompleted = reader.GetBoolean(reader.GetOrdinal("IsCompleted")),
                    Salaries = await ReadSalariesAsync(idNumber, cancellationToken) // Populate Salaries
                };

                return salaryProject;
            }

            return null; // Return null if no matching salary project is found
        }
        public async Task<SalaryProjectRequest?> ReadSalaryProjectRequestAsync(string login, string companyName, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
        SELECT IdNumber, Login, CompanyName, AccountId, Salary, IsApproved
        FROM salary_project_request
        WHERE Login = @Login AND CompanyName = @CompanyName
        """;

            await using var command = new NpgsqlCommand(SQLquery, connection);
            command.Parameters.AddWithValue("@Login", login);
            command.Parameters.AddWithValue("@CompanyName", companyName);

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);

            if (await reader.ReadAsync(cancellationToken))
            {
                var request = new SalaryProjectRequest
                {
                    IdNumber = reader.GetInt32(reader.GetOrdinal("IdNumber")),
                    Login = reader.GetString(reader.GetOrdinal("Login")),
                    CompanyName = reader.GetString(reader.GetOrdinal("CompanyName")),
                    Salary = reader.GetDecimal(reader.GetOrdinal("Salary")),
                    IsApproved = reader.GetBoolean(reader.GetOrdinal("IsApproved")),
                    Account = new Account
                    {
                        Bank = new Bank
                        {
                            Name = ""
                        }
                    }
                };

                // Fetch the associated Account using existing method
                int accountId = reader.GetInt32(reader.GetOrdinal("AccountId"));
                request.Account = await ReadAccountAsync(accountId, cancellationToken) ?? throw new Exception("No Account for SalaryProjectRequest");

                return request;
            }

            return null; // Return null if no matching request is found
        }
        public async Task ApproveSalaryProjectRequestAsync(SalaryProjectRequest salaryProjectRequest, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
        UPDATE salary_project_request
        SET IsApproved = true
        WHERE IdNumber = @IdNumber
        """;

            await using var command = new NpgsqlCommand(SQLquery, connection);
            command.Parameters.AddWithValue("@IdNumber", salaryProjectRequest.IdNumber);

            await command.ExecuteNonQueryAsync(cancellationToken);
        }
        public async Task AddSalaryAsync(SalaryProjectRequest salaryProjectRequest, SalaryProject salaryProject, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
        INSERT INTO salaries
        (SalaryProjectId, Salary, AccountId)
        VALUES
        (@SalaryProjectId, @Salary, @AccountId)
        """;

            await using var command = new NpgsqlCommand(SQLquery, connection);

            command.Parameters.AddWithValue("@SalaryProjectId", salaryProject.IdNumber);
            command.Parameters.AddWithValue("@Salary", salaryProjectRequest.Salary);
            command.Parameters.AddWithValue("@AccountId", salaryProjectRequest.Account.IdNumber);

            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }
}
