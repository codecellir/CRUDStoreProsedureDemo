using CRUDStoreProsedureDemo.Models;
using Microsoft.Data.SqlClient;
using System.Data;
namespace CRUDStoreProsedureDemo.Services
{
    public class ProductService(IConfiguration configuration) : IProductService
    {
        private string connectionString = configuration.GetConnectionString("DefaultConnection")!;
        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("DeleteProduct", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@Id", id);
            await connection.OpenAsync();
            var effectedRows = await command.ExecuteNonQueryAsync();

            if (effectedRows == 0)
                throw new Exception("Failed To Delete");
        }

        public async Task<Product> GetAsync(int id)
        {
            Product product = null;
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("GetProduct", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@Id", id);
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                product = new()
                {
                    Id = reader.GetInt32("Id"),
                    Name = reader.GetString("Name"),
                    Price = reader.GetDecimal("Price")
                };
            }
            if (product is null)
                throw new Exception("Not Found!");

            return product;
        }

        public async Task InsertAsync(Product product)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("InsertProduct", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@Name", product.Name);
            command.Parameters.AddWithValue("@Price", product.Price);
            await connection.OpenAsync();
            var effectedRows = await command.ExecuteNonQueryAsync();

            if (effectedRows == 0)
                throw new Exception("Failed To Insert");
        }

        public async Task<IReadOnlyList<Product>> ListAsync()
        {
            List<Product> products = [];

            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("GetProducts", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                products.Add(new()
                {
                    Id = reader.GetInt32("Id"),
                    Name = reader.GetString("Name"),
                    Price = reader.GetDecimal("Price")
                });
            }

            return products;
        }

        public async Task UpdateAsync(Product product)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("UpdateProduct", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@Id", product.Id);
            command.Parameters.AddWithValue("@Name", product.Name);
            command.Parameters.AddWithValue("@Price", product.Price);
            await connection.OpenAsync();
            var effectedRows = await command.ExecuteNonQueryAsync();

            if (effectedRows == 0)
                throw new Exception("Failed To Update");
        }
    }
}
