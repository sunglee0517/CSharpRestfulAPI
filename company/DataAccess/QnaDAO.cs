using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class QnaDAO
{
    private readonly string _connectionString;

    public QnaDAO(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<List<Qna>> GetAllQnasAsync()
    {
        List<Qna> qnas = new List<Qna>();

        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = "SELECT * FROM qna";

            using (var command = new MySqlCommand(query, connection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        qnas.Add(new Qna
                        {
                            Qno = Convert.ToInt32(reader["qno"]),
                            Lev = Convert.ToInt32(reader["lev"]),
                            Parno = reader["parno"] != DBNull.Value ? Convert.ToInt32(reader["parno"]) : (int?)null,
                            Title = reader["title"].ToString(),
                            Content = reader["content"].ToString(),
                            Author = reader["author"].ToString(),
                            ResDate = Convert.ToDateTime(reader["resdate"]),
                            Hits = Convert.ToInt32(reader["hits"])
                        });
                    }
                }
            }
        }

        return qnas;
    }

    public async Task<Qna> GetQnaByIdAsync(int id)
    {
        Qna qna = null;

        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = "SELECT * FROM qna WHERE qno = @id";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        qna = new Qna
                        {
                            Qno = Convert.ToInt32(reader["qno"]),
                            Lev = Convert.ToInt32(reader["lev"]),
                            Parno = reader["parno"] != DBNull.Value ? Convert.ToInt32(reader["parno"]) : (int?)null,
                            Title = reader["title"].ToString(),
                            Content = reader["content"].ToString(),
                            Author = reader["author"].ToString(),
                            ResDate = Convert.ToDateTime(reader["resdate"]),
                            Hits = Convert.ToInt32(reader["hits"])
                        };
                    }
                }
            }
        }

        return qna;
    }

    public async Task<Qna> InsertQnaAsync(Qna qna)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = @"INSERT INTO qna (lev, parno, title, content, author, resdate, hits) 
                             VALUES (@lev, @parno, @title, @content, @author,
                                     @resdate, @hits);
                             SELECT LAST_INSERT_ID();";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@lev", qna.Lev);
                command.Parameters.AddWithValue("@parno", qna.Parno ?? (object)DBNull.Value); // Handle nullable parno
                command.Parameters.AddWithValue("@title", qna.Title);
                command.Parameters.AddWithValue("@content", qna.Content);
                command.Parameters.AddWithValue("@author", qna.Author);
                command.Parameters.AddWithValue("@resdate", DateTime.Now);
                command.Parameters.AddWithValue("@hits", 0);

                // ExecuteScalarAsync to get the inserted ID
                int insertedId = Convert.ToInt32(await command.ExecuteScalarAsync());
                qna.Qno = insertedId; // Set the ID of the inserted qna
            }
        }

        return qna;
    }

    public async Task<bool> UpdateQnaAsync(Qna qna)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = @"UPDATE qna 
                             SET lev = @lev, 
                                 parno = @parno,
                                 title = @title, 
                                 content = @content, 
                                 author = @author,
                                 resdate = @resdate,
                                 hits = @hits
                             WHERE qno = @qno";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@lev", qna.Lev);
                command.Parameters.AddWithValue("@parno", qna.Parno ?? (object)DBNull.Value); // Handle nullable parno
                command.Parameters.AddWithValue("@title", qna.Title);
                command.Parameters.AddWithValue("@content", qna.Content);
                command.Parameters.AddWithValue("@author", qna.Author);
                command.Parameters.AddWithValue("@resdate", qna.ResDate);
                command.Parameters.AddWithValue("@hits", qna.Hits);
                command.Parameters.AddWithValue("@qno", qna.Qno);

                int rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }
    }

    public async Task<bool> DeleteQnaAsync(int id)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = @"DELETE FROM qna 
                             WHERE qno = @qno";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@qno", id);

                int rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }
    }
}