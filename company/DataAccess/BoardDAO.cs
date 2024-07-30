using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class BoardDAO
{
    private readonly string _connectionString;

    public BoardDAO(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<List<Board>> GetAllBoardsAsync()
    {
        List<Board> boards = new List<Board>();

        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = "SELECT * FROM board";

            using (var command = new MySqlCommand(query, connection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        boards.Add(new Board
                        {
                            No = Convert.ToInt32(reader["no"]),
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

        return boards;
    }

    public async Task<Board> GetBoardByIdAsync(int id)
    {
        Board board = null;

        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = "SELECT * FROM board WHERE no = @id";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        board = new Board
                        {
                            No = Convert.ToInt32(reader["no"]),
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

        return board;
    }

    public async Task<Board> InsertBoardAsync(Board board)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = @"INSERT INTO board (title, content, author, resdate, hits) 
                             VALUES (@title, @content, @author, 
                                     @resdate, @hits);
                             SELECT LAST_INSERT_ID();";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@title", board.Title);
                command.Parameters.AddWithValue("@content", board.Content);
                command.Parameters.AddWithValue("@author", board.Author);
                command.Parameters.AddWithValue("@resdate", DateTime.Now);
                command.Parameters.AddWithValue("@hits", 0);

                // ExecuteScalarAsync to get the inserted ID
                int insertedId = Convert.ToInt32(await command.ExecuteScalarAsync());
                board.No = insertedId; // Set the ID of the inserted board
            }
        }

        return board;
    }

    public async Task<bool> UpdateBoardAsync(Board board)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = @"UPDATE board 
                             SET title = @title, 
                                 content = @content, 
                                 author = @author,
                                 resdate = @resdate,
                                 hits = @hits
                             WHERE no = @no";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@title", board.Title);
                command.Parameters.AddWithValue("@content", board.Content);
                command.Parameters.AddWithValue("@author", board.Author);
                command.Parameters.AddWithValue("@resdate", board.ResDate);
                command.Parameters.AddWithValue("@hits", board.Hits);
                command.Parameters.AddWithValue("@no", board.No);

                int rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }
    }

    public async Task<bool> DeleteBoardAsync(int id)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = @"DELETE FROM board 
                             WHERE no = @no";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@no", id);

                int rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }
    }
}