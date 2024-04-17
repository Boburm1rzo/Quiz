using Npgsql;
using Telegram.Bot.Types;
using Quiz.Models;

namespace Quiz.Brokers.SubjectBrokers;

internal class SubjectBroker : ISubjectBroker
{
    public async Task<bool> InsertSubjectAsync(string name)
    {
        try
        {
            NpgsqlConnection _connection = Helper.GetConnection();
            await _connection.OpenAsync();

            string query = "INSERT INTO quizz_schema.subject(name) Values (@name);";

            using (NpgsqlCommand command = new NpgsqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@name", name);

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

    public async Task<bool> DeleteSubjectAsync(int subjectId)
    {
        try
        {
            NpgsqlConnection _connection = Helper.GetConnection();
            await _connection.OpenAsync();

            string query = "delete from quizz_schema.subject where id = @id";

            using (NpgsqlCommand command = new NpgsqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@id", subjectId);

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

    public async Task<List<Subject>> SelectSubjectAsync()
    {
        try
        {
            NpgsqlConnection _connection = Helper.GetConnection();
            await _connection.OpenAsync();

            string query = "select * from quizz_schema.subject";

            using (NpgsqlCommand command = new NpgsqlCommand(query, _connection))
            {

                NpgsqlDataReader storedSubjets = await command.ExecuteReaderAsync();

                List<Subject> subjects = new List<Subject>();
                while(await storedSubjets.ReadAsync())
                {
                    subjects.Add(
                        new Subject()
                        {
                            Id = storedSubjets.GetInt32(0),
                            Name = storedSubjets.GetString(1)
                        }
                        );
                }

                await _connection.CloseAsync();
                return subjects;
                
            }
        }
        catch (NpgsqlException ex)
        {
            throw new Exception("Database error", ex);
        }
    }

    public Task<bool> UpdateSubjectAsync(Subject subject)
    {
        throw new NotImplementedException();
    }
}
