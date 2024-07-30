using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ProductDAO
{
    private readonly string _connectionString;

    public ProductDAO(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
        List<Product> products = new List<Product>();

        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = "SELECT * FROM product";

            using (var command = new MySqlCommand(query, connection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        products.Add(new Product
                        {
                            Pno = Convert.ToInt32(reader["pno"]),
                            Cate = reader["cate"].ToString(),
                            Pname = reader["pname"].ToString(),
                            Pcontent = reader["pcontent"].ToString(),
                            Img1 = reader["img1"].ToString(),
                            Img2 = reader["img2"].ToString(),
                            Img3 = reader["img3"].ToString(),
                            ResDate = Convert.ToDateTime(reader["resdate"]),
                            Hits = Convert.ToInt32(reader["hits"])
                        });
                    }
                }
            }
        }

        return products;
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        Product product = null;

        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = "SELECT * FROM product WHERE pno = @id";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        product = new Product
                        {
                            Pno = Convert.ToInt32(reader["pno"]),
                            Cate = reader["cate"].ToString(),
                            Pname = reader["pname"].ToString(),
                            Pcontent = reader["pcontent"].ToString(),
                            Img1 = reader["img1"].ToString(),
                            Img2 = reader["img2"].ToString(),
                            Img3 = reader["img3"].ToString(),
                            ResDate = Convert.ToDateTime(reader["resdate"]),
                            Hits = Convert.ToInt32(reader["hits"])
                        };
                    }
                }
            }
        }

        return product;
    }

    public async Task<Product> InsertProductAsync(Product product)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = @"INSERT INTO product (cate, pname, pcontent, img1, img2, img3, resdate, hits) 
                             VALUES (@cate, @pname, @pcontent, @img1, @img2, @img3, @resdate, @hits);
                             SELECT LAST_INSERT_ID();";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@cate", product.Cate);
                command.Parameters.AddWithValue("@pname", product.Pname);
                command.Parameters.AddWithValue("@pcontent", product.Pcontent);
                command.Parameters.AddWithValue("@img1", product.Img1);
                command.Parameters.AddWithValue("@img2", product.Img2);
                command.Parameters.AddWithValue("@img3", product.Img3);
                command.Parameters.AddWithValue("@resdate", DateTime.Now);
                command.Parameters.AddWithValue("@hits", 0);

                // ExecuteScalarAsync to get the inserted ID
                int insertedId = Convert.ToInt32(await command.ExecuteScalarAsync());
                product.Pno = insertedId; // Set the ID of the inserted product
            }
        }

        return product;
    }

    public async Task<bool> UpdateProductAsync(Product product)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = @"UPDATE product 
                             SET cate = @cate, 
                                 pname = @pname, 
                                 pcontent = @pcontent,
                                 img1 = @img1,
                                 img2 = @img2,
                                 img3 = @img3,
                                 resdate = @resdate,
                                 hits = @hits
                             WHERE pno = @pno";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@cate", product.Cate);
                command.Parameters.AddWithValue("@pname", product.Pname);
                command.Parameters.AddWithValue("@pcontent", product.Pcontent);
                command.Parameters.AddWithValue("@img1", product.Img1);
                command.Parameters.AddWithValue("@img2", product.Img2);
                command.Parameters.AddWithValue("@img3", product.Img3);
                command.Parameters.AddWithValue("@resdate", product.ResDate);
                command.Parameters.AddWithValue("@hits", product.Hits);
                command.Parameters.AddWithValue("@pno", product.Pno);

                int rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = @"DELETE FROM product 
                             WHERE pno = @pno";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@pno", id);

                int rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }
    }
}