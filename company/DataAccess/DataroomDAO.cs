using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class DataroomDAO
{
    private readonly string _connectionString;

    public DataroomDAO(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<List<Dataroom>> GetAllDataroomsAsync()
    {
        List<Dataroom> datarooms = new List<Dataroom>();

        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = "SELECT * FROM dataroom";

            using (var command = new MySqlCommand(query, connection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        datarooms.Add(new Dataroom
                        {
                            Dno = Convert.ToInt32(reader["dno"]),
                            Title = reader["title"].ToString(),
                            Content = reader["content"].ToString(),
                            Author = reader["author"].ToString(),
                            DataFile = reader["datafile"].ToString(),
                            ResDate = Convert.ToDateTime(reader["resdate"]),
                            Hits = Convert.ToInt32(reader["hits"])
                        });
                    }
                }
            }
        }

        return datarooms;
    }

    public async Task<Dataroom> GetDataroomByIdAsync(int id)
    {
        Dataroom dataroom = null;

        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = "SELECT * FROM dataroom WHERE dno = @id";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        dataroom = new Dataroom
                        {
                            Dno = Convert.ToInt32(reader["dno"]),
                            Title = reader["title"].ToString(),
                            Content = reader["content"].ToString(),
                            Author = reader["author"].ToString(),
                            DataFile = reader["datafile"].ToString(),
                            ResDate = Convert.ToDateTime(reader["resdate"]),
                            Hits = Convert.ToInt32(reader["hits"])
                        };
                    }
                }
            }
        }

        return dataroom;
    }

    public async Task<Dataroom> InsertDataroomAsync(Dataroom dataroom)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = @"INSERT INTO dataroom (title, content, author, datafile, resdate, hits) 
                             VALUES (@title, @content, @author,
                                     @datafile, @resdate, @hits);
                             SELECT LAST_INSERT_ID();";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@title", dataroom.Title);
                command.Parameters.AddWithValue("@content", dataroom.Content);
                command.Parameters.AddWithValue("@author", dataroom.Author);
                command.Parameters.AddWithValue("@datafile", dataroom.DataFile);
                command.Parameters.AddWithValue("@resdate", DateTime.Now);
                command.Parameters.AddWithValue("@hits", 0);

                // ExecuteScalarAsync to get the inserted ID
                int insertedId = Convert.ToInt32(await command.ExecuteScalarAsync());
                dataroom.Dno = insertedId; // Set the ID of the inserted dataroom
            }
        }

        return dataroom;
    }

    public async Task<bool> UpdateDataroomAsync(Dataroom dataroom)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = @"UPDATE dataroom 
                             SET title = @title, 
                                 content = @content, 
                                 author = @author,
                                 datafile = @datafile,
                                 resdate = @resdate,
                                 hits = @hits
                             WHERE dno = @dno";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@title", dataroom.Title);
                command.Parameters.AddWithValue("@content", dataroom.Content);
                command.Parameters.AddWithValue("@author", dataroom.Author);
                command.Parameters.AddWithValue("@datafile", dataroom.DataFile);
                command.Parameters.AddWithValue("@resdate", dataroom.ResDate);
                command.Parameters.AddWithValue("@hits", dataroom.Hits);
                command.Parameters.AddWithValue("@dno", dataroom.Dno);

                int rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }
    }

    public async Task<bool> DeleteDataroomAsync(int id)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = @"DELETE FROM dataroom 
                             WHERE dno = @dno";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@dno", id);

                int rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }
    }
}