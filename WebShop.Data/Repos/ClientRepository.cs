using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using WebShop.Data.Entities;
using WebShop.Data.Interfaces;
using System.Linq;

namespace WebShop.Data.Repos
{
    public class ClientRepository : IClientRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string connectionString;

        public ClientRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("AppDbConnection");
        }

        public async Task<int> Add(Client entity)
        {
            string sql = @"INSERT INTO Clients (Id, FirstName, LastName, PhoneNumber, Address)
                           VALUES (@Id, @FirstName, @LastName, @PhoneNumber, @Address);";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                int affectedRows = await connection.ExecuteAsync(sql, entity);
                return affectedRows;
            }
        }

        public async Task<Client> Get(string id)
        {
            string sql = @"Select * FROM Clients WHERE Id = @Id;";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var result = await connection.QueryAsync<Client>(sql, new {Id = id});
                return result.FirstOrDefault();
            }
        }

        public async Task<IEnumerable<Client>> GetAll()
        {
            string sql = @"Select * FROM Clients;";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var result = await connection.QueryAsync<Client>(sql);
                return result;
            }
        }

        public async Task<int> Remove(string id)
        {
            var sql = "DELETE FROM Clients WHERE Id = @Id;";
            await using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
                return affectedRows;
            }
        }

        public async Task<int> Update(Client entity)
        {
            var sql = "UPDATE Tasks SET Id = @Id, FirstName = @FirstName, LastName = @LastName, PhoneNumber = @PhoneNumber, Address = @Address WHERE Id = @Id;";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var affectedRows = await connection.ExecuteAsync(sql, entity);
                return affectedRows;
            }
        }
    }
}
