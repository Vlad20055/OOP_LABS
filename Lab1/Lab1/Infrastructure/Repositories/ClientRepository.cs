﻿using System.Security.Principal;
using Lab1.Domain;
using Lab1.Domain.BankServices;
using Lab1.Domain.Repositories;
using Lab1.Domain.Users;
using Lab1.Infrastructure.Options;
using Npgsql;


namespace Lab1.Infrastructure.Repositories
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
            command.Parameters.AddWithValue("@IsApproved", client.IsApproved);
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

        public async Task<Client?> ReadAsync(string login, CancellationToken cancellationToken)
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

            var client = new Client(new ClientRepository());

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
            client.IsApproved = (bool)reader["IsApproved"];
            client.Role = UserRole.Clilent;
            client.Accounts = ReadAccountsByClientAsync(login, cancellationToken).Result;
            client.Credits = ReadCreditsByClientAsync(login, cancellationToken).Result;
            

            return client;
        }

        /*********************************************ACCOUNT*********************************************/

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

        public async Task AddClientAccountRecordAsync(Client client, Account account, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
                INSERT INTO client_account_records
                (Login, IdNumber)
                VALUES 
                (@Login, @IdNumber)
                """;

            var command = new NpgsqlCommand(SQLquery, connection);

            command.Parameters.AddWithValue("@Login", client.Login);
            command.Parameters.AddWithValue("@IdNumber", account.IdNumber);

            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        public async Task<List<Account>> ReadAccountsByClientAsync(string login, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
                SELECT a.IdNumber, a.Amount, a.IsActive, b.Id AS BankId, b.Name AS BankName
                FROM accounts a
                JOIN client_account_records car ON a.IdNumber = car.IdNumber
                JOIN account_bank_records abr ON a.IdNumber = abr.IdNumber
                JOIN banks b ON abr.Name = b.Name
                WHERE car.Login = @Login;
                """;

            await using var command = new NpgsqlCommand(SQLquery, connection);
            command.Parameters.AddWithValue("@Login", login);

            var accounts = new List<Account>();

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            while (await reader.ReadAsync(cancellationToken))
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
                accounts.Add(account);
            }

            return accounts;
        }

        public async Task RemoveClientAccountRecordAsync(Client client, Account account, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
                DELETE FROM client_account_records
                WHERE Login = @Login AND IdNumber = @IdNumber;
                """;

            await using var command = new NpgsqlCommand(SQLquery, connection);
            command.Parameters.AddWithValue("@Login", client.Login);
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

        /*********************************************CREDIT*********************************************/

        public async Task CreateCreditAsync(Credit credit, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
                INSERT INTO credits (Id, IsApproved, CreditPeriod, Persent, Rest)
                VALUES (@Id, @IsApproved, @CreditPeriod, @Persent, @Rest)
                RETURNING IdNumber;
                """;

            await using var command = new NpgsqlCommand(SQLquery, connection);

            command.Parameters.AddWithValue("@Id", credit.Bank?.Id ?? throw new ArgumentNullException(nameof(credit.Bank), "Bank ID cannot be null."));
            command.Parameters.AddWithValue("@IsApproved", credit.IsApproved);
            command.Parameters.AddWithValue("@CreditPeriod", (int)credit.Period);
            command.Parameters.AddWithValue("@Persent", credit.Persent);
            command.Parameters.AddWithValue("@Rest", credit.Rest);

            var result = await command.ExecuteScalarAsync(cancellationToken);

            if (result == null)
            {
                throw new NpgsqlException("Failed to create credit. No ID was returned.");
            }

            credit.IdNumber = (int)result;
        }

        public async Task DeleteCreditAsync(Credit credit, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
                DELETE FROM credits
                WHERE IdNumber = @IdNumber;
                """;

            await using var command = new NpgsqlCommand(SQLquery, connection);

            command.Parameters.AddWithValue("@IdNumber", credit.IdNumber);

            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        public async Task AddClientCreditRecordAsync(Client client, Credit credit, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
                INSERT INTO client_credit_records
                (Login, IdNumber)
                VALUES 
                (@Login, @IdNumber)
                """;

            var command = new NpgsqlCommand(SQLquery, connection);

            command.Parameters.AddWithValue("@Login", client.Login);
            command.Parameters.AddWithValue("@IdNumber", credit.IdNumber);

            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        public async Task<List<Credit>> ReadCreditsByClientAsync(string login, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
                SELECT c.IdNumber, c.Id AS BankId, c.IsApproved, c.CreditPeriod, c.Persent, c.Rest
                FROM credits c
                JOIN client_credit_records ccr ON c.IdNumber = ccr.IdNumber
                WHERE ccr.Login = @Login;
                """;

            await using var command = new NpgsqlCommand(SQLquery, connection);
            command.Parameters.AddWithValue("@Login", login);

            var credits = new List<Credit>();

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            while (await reader.ReadAsync(cancellationToken))
            {
                var credit = new Credit
                {
                    IdNumber = reader.GetInt32(reader.GetOrdinal("IdNumber")),
                    Bank = (new BankRepository()).ReadAsync(reader.GetInt32(reader.GetOrdinal("BankId")), cancellationToken).Result,
                    IsApproved = reader.GetBoolean(reader.GetOrdinal("IsApproved")),
                    Period = (CreditPeriod)reader.GetInt32(reader.GetOrdinal("CreditPeriod")),
                    Persent = reader.GetDecimal(reader.GetOrdinal("Persent")),
                    Rest = reader.GetDecimal(reader.GetOrdinal("Rest"))
                };
            //Console.WriteLine(1);
                credits.Add(credit);
            }

            return credits;
        }

        public async Task RemoveClientCreditRecordAsync(Client client, Credit credit, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
                DELETE FROM client_credit_records
                WHERE Login = @Login AND IdNumber = @IdNumber;
                """;

            await using var command = new NpgsqlCommand(SQLquery, connection);
            command.Parameters.AddWithValue("@Login", client.Login);
            command.Parameters.AddWithValue("@IdNumber", credit.IdNumber);

            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }
}
