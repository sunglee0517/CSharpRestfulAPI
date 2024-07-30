using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class MemberDAO
{
    private readonly string _connectionString;

    public MemberDAO(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<List<Member>> GetAllMembersAsync()
    {
        List<Member> members = new List<Member>();

        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = "SELECT * FROM member";

            using (var command = new MySqlCommand(query, connection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        members.Add(new Member
                        {
                            Id = reader["id"].ToString(),
                            Pw = reader["pw"].ToString(),
                            Name = reader["name"].ToString(),
                            Birth = Convert.ToDateTime(reader["birth"]),
                            Email = reader["email"].ToString(),
                            Tel = reader["tel"].ToString(),
                            Addr1 = reader["addr1"].ToString(),
                            Addr2 = reader["addr2"].ToString(),
                            Postcode = reader["postcode"].ToString(),
                            RegDate = Convert.ToDateTime(reader["regdate"])
                        });
                    }
                }
            }
        }

        return members;
    }

    public async Task<Member> GetMemberByIdAsync(string id)
    {
        Member member = null;

        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = "SELECT * FROM member WHERE id = @id";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        member = new Member
                        {
                            Id = reader["id"].ToString(),
                            Pw = reader["pw"].ToString(),
                            Name = reader["name"].ToString(),
                            Birth = Convert.ToDateTime(reader["birth"]),
                            Email = reader["email"].ToString(),
                            Tel = reader["tel"].ToString(),
                            Addr1 = reader["addr1"].ToString(),
                            Addr2 = reader["addr2"].ToString(),
                            Postcode = reader["postcode"].ToString(),
                            RegDate = Convert.ToDateTime(reader["regdate"])
                        };
                    }
                }
            }
        }

        return member;
    }

    public async Task<Member> InsertMemberAsync(Member member)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = @"INSERT INTO member (id, pw, name, birth, email, tel, addr1, addr2, postcode, regdate) 
                             VALUES (@id, @pw, @name, @birth, @email, @tel, @addr1, @addr2, @postcode, @regdate)";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", member.Id);
                command.Parameters.AddWithValue("@pw", member.Pw);
                command.Parameters.AddWithValue("@name", member.Name);
                command.Parameters.AddWithValue("@birth", member.Birth);
                command.Parameters.AddWithValue("@email", member.Email);
                command.Parameters.AddWithValue("@tel", member.Tel);
                command.Parameters.AddWithValue("@addr1", member.Addr1);
                command.Parameters.AddWithValue("@addr2", member.Addr2);
                command.Parameters.AddWithValue("@postcode", member.Postcode);
                command.Parameters.AddWithValue("@regdate", DateTime.Now);

                await command.ExecuteNonQueryAsync();
            }
        }

        return member;
    }

    public async Task<bool> UpdateMemberAsync(Member member)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = @"UPDATE member 
                             SET pw = @pw, 
                                 name = @name, 
                                 birth = @birth,
                                 email = @email,
                                 tel = @tel,
                                 addr1 = @addr1,
                                 addr2 = @addr2,
                                 postcode = @postcode
                             WHERE id = @id";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@pw", member.Pw);
                command.Parameters.AddWithValue("@name", member.Name);
                command.Parameters.AddWithValue("@birth", member.Birth);
                command.Parameters.AddWithValue("@email", member.Email);
                command.Parameters.AddWithValue("@tel", member.Tel);
                command.Parameters.AddWithValue("@addr1", member.Addr1);
                command.Parameters.AddWithValue("@addr2", member.Addr2);
                command.Parameters.AddWithValue("@postcode", member.Postcode);
                command.Parameters.AddWithValue("@id", member.Id);

                int rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }
    }

    public async Task<bool> DeleteMemberAsync(string id)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = @"DELETE FROM member 
                             WHERE id = @id";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);

                int rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }
    }
}