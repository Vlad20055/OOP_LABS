

using Lab1.Domain;
using Lab1.Domain.BankServices;
using Lab1.Domain.Repositories;
using Lab1.Infrastructure.Options;
using Npgsql;

namespace Lab1.Infrastructure.Repositories
{
    internal class CompanyRepository : ICompanyRepository
    {
        public async Task CreateAsync(Company company, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
                INSERT INTO companies (CompanyType, Name, PayerAccountNumber, BankIdentificationCode, Address, SalaryProjectId)
                VALUES (@CompanyType, @Name, @PayerAccountNumber, @BankIdentificationCode, @Address, @SalaryProjectId)
                RETURNING Id;
                """;

            await using var command = new NpgsqlCommand(SQLquery, connection);

            // Map properties to parameters
            command.Parameters.AddWithValue("@CompanyType", (int)company.CompanyType); // Cast enum to int
            command.Parameters.AddWithValue("@Name", company.Name);
            command.Parameters.AddWithValue("@PayerAccountNumber", company.Account.IdNumber); // Use Account.IdNumber
            command.Parameters.AddWithValue("@BankIdentificationCode", company.Account.Bank.Id);
            command.Parameters.AddWithValue("@Address", company.Address);

            // Handle optional SalaryProjectId
            if (company.SalaryProject != null)
            {
                command.Parameters.AddWithValue("@SalaryProjectId", company.SalaryProject.IdNumber);
            }
            else
            {
                command.Parameters.AddWithValue("@SalaryProjectId", DBNull.Value); // Insert NULL if SalaryProject is null
            }

            // Execute the query and retrieve the auto-generated Id
            var result = await command.ExecuteScalarAsync(cancellationToken);

            if (result == null)
            {
                throw new NpgsqlException("Failed to create company. No ID was returned.");
            }

            // Assign the auto-generated Id to the Company object
            company.Id = (int)result;
        }

        public async Task DeleteAsync(Company company, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
                DELETE FROM companies
                WHERE Id = @Id;
                """;

            await using var command = new NpgsqlCommand(SQLquery, connection);

            command.Parameters.AddWithValue("@Id", company.Id);

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


        public async Task<Company?> ReadAsync(string name, CancellationToken cancellationToken)
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


        public async Task CreateAccountAsync(Account account, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
                INSERT INTO accounts
                (Amount, IsActive)
                VALUES 
                (@Amount, @IsActive)
                RETURNING IdNumber
                """;

            var command = new NpgsqlCommand(SQLquery, connection);

            command.Parameters.AddWithValue("@Amount", account.Amount);
            command.Parameters.AddWithValue("@IsActive", account.IsActive);

            var result = await command.ExecuteScalarAsync(cancellationToken);

            if (result == null)
            {
                throw new NpgsqlException("Failed to create account. No ID was returned.");
            }

            account.IdNumber = (int)result;
        }

        public async Task DeleteAccountAsync(Account account, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
                DELETE FROM accounts
                WHERE IdNumber = @IdNumber
                """;

            var command = new NpgsqlCommand(SQLquery, connection);

            command.Parameters.AddWithValue("@IdNumber", account.IdNumber);

            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        public async Task AddAccountBankRecordAsync(Account account, Bank bank, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
                INSERT INTO account_bank_records
                (IdNumber, Name)
                VALUES 
                (@IdNumber, @Name)
                """;

            var command = new NpgsqlCommand(SQLquery, connection);

            command.Parameters.AddWithValue("@IdNumber", account.IdNumber);
            command.Parameters.AddWithValue("@Name", bank.Name);

            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        public async Task RemoveAccountBankRecordAsync(Account account, Bank bank, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
                DELETE FROM account_bank_records
                WHERE IdNumber = @IdNumber AND Name = @Name;
                """;

            await using var command = new NpgsqlCommand(SQLquery, connection);
            command.Parameters.AddWithValue("@IdNumber", account.IdNumber);
            command.Parameters.AddWithValue("@Name", bank.Name);

            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }
}
