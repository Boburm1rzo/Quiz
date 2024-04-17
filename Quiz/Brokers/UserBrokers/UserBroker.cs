using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;
using System.Data;
using Telegram.Bot.Types;
using Quiz.Brokers.UserBroker;
using Quiz.Models;

namespace Quiz.Brokers.UserBrokers;

internal class UserBroker : IUserBroker
{   
    public async Task<Role?> LoginAsync(long tgId)
    {
        try
        {
            NpgsqlConnection _connection = Helper.GetConnection();
            await _connection.OpenAsync();
                        
            string query = "Select user_role from user_schema.auth_user where tg_id = @tg_id";

            using (NpgsqlCommand command = new NpgsqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@tg_id", tgId);

                NpgsqlDataReader reader  = await command.ExecuteReaderAsync();

                Role? role = null;
                while(await reader.ReadAsync())
                {
                    role = (reader[0].ToString() == "ADMIN") ? Role.ADMIN : Role.USER;
                }

                await _connection.CloseAsync();

                return role;
            }
        }
        catch (NpgsqlException ex)
        {
            throw new Exception("Database error", ex);
        }
    }

    public async Task<bool> RegisterUserAsync(TgUser user)
    {
        try
        {
            NpgsqlConnection _connection = Helper.GetConnection();
            await _connection.OpenAsync();

            string query = "INSERT INTO user_schema.auth_user(tg_id, username, full_name) Values (@tg_id, @user_name, @full_name);";

            using (NpgsqlCommand command = new NpgsqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@tg_id", user.TgId);
                command.Parameters.AddWithValue("@user_name", user.UserName);
                command.Parameters.AddWithValue("@full_name", user.FullName);

                int effectedRow = await command.ExecuteNonQueryAsync();

                await _connection.CloseAsync();
                return effectedRow > 0;
            }
        }
        catch (NpgsqlException ex)
        {
            throw new Exception("Database error", ex);
        }
    }
}
