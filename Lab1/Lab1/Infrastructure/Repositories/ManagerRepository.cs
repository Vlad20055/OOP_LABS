﻿using Npgsql;
using Lab1.Infrastructure.Options;
using Lab1.Domain.Users;
using Lab1.Domain.Repositories;
using Lab1.Domain.BankServices;

namespace Lab1.Infrastructure.Repositories
{
    internal class ManagerRepository : IManagerRepository
    {
        public async Task CreateAsync(Manager manager, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
                INSERT INTO managers
                (IdNumber, Name, Login, Password)
                VALUES
                (@IdNumber, @Name, @Login, @Password)
                RETURNING Login
                """;

            var command = new NpgsqlCommand(SQLquery, connection);

            command.Parameters.AddWithValue("@IdNumber", manager.IdNumber);
            command.Parameters.AddWithValue("@Name", manager.Name);
            command.Parameters.AddWithValue("@Login", manager.Login);
            command.Parameters.AddWithValue("@Password", manager.Password);

            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        public async Task<Manager?> ReadAsync(string login, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
                SELECT * FROM managers
                WHERE Login = @Login
                """;

            var command = new NpgsqlCommand(SQLquery, connection);

            command.Parameters.AddWithValue("@Login", login);

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);


            if (!await reader.ReadAsync(cancellationToken)) return null;

            var manager = new Manager(this)
            {
                IdNumber = (string)reader["IdNumber"],
                Name = (string)reader["Name"],
                Login = (string)reader["Login"],
                Password = (string)reader["Password"],
                Role = UserRole.Manager
            };

            return manager;
        }

        public async Task DeleteAsync(Manager manager, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
                DELETE FROM managers
                WHERE Login = @Login
                """;

            var command = new NpgsqlCommand(SQLquery, connection);

            command.Parameters.AddWithValue("@Login", manager.Login);

            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        public async Task<List<Client>> ReadAllNotApprovedClientsAsync(CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
                SELECT * FROM clients
                WHERE IsApproved = @IsApproved
                """;

            var command = new NpgsqlCommand(SQLquery, connection);

            command.Parameters.AddWithValue("@IsApproved", false);

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);

            var clients = new List<Client>();

            while (await reader.ReadAsync(cancellationToken))
            {
                clients.Add(new Client(new ClientRepository(), new TransferRepository())
                {
                    Surname = (string)reader["Surname"],
                    Name = (string)reader["Name"],
                    Patronymic = (string)reader["Patronymic"],
                    PassportSeriesAndNumber = (string)reader["PassportSeriesAndNumber"],
                    IdNumber = (string)reader["IdNumber"],
                    PhoneNumber = (string)reader["PhoneNumber"],
                    Email = (string)reader["Email"],
                    Login = (string)reader["Login"],
                    Password = (string)reader["Password"],
                });
            }

            return clients;
        }

        public async Task ApproveClientAsync(Client client, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string SQLquery = """
                UPDATE clients
                SET IsApproved = @IsApproved
                WHERE Login = @Login
                """;

            var command = new NpgsqlCommand(SQLquery, connection);

            command.Parameters.AddWithValue("@Login", client.Login);
            command.Parameters.AddWithValue("@IsApproved", true);

            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        public async Task ApproveClientBankAbilityAsync(BankAbility bankAbility, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(DatabaseOptions.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            string table = "";

            switch (bankAbility.Type)
            {
                case AbilityType.Credit:
                    table = "credits";
                    break;
                case AbilityType.Installment:
                    table = "installments";
                    break;
                case AbilityType.Deposit:
                    table = "deposits";
                    break;
                default:
                    throw new Exception("No such AbilityType!");
            }

            string SQLquery = "UPDATE " + table +
                """

                SET IsApproved = @IsApproved
                WHERE IdNumber = @IdNumber
                """;

            var command = new NpgsqlCommand(SQLquery, connection);

            command.Parameters.AddWithValue("@IdNumber", bankAbility.IdNumber);
            command.Parameters.AddWithValue("@IsApproved", true);

            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }
}
