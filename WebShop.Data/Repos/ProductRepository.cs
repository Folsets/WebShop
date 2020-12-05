using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using WebShop.Data.Entities;
using WebShop.Data.Interfaces;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace WebShop.Data.Repos
{
    public class ProductRepository : IProductRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string connectionString;

        public ProductRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("AppDbConnection");
        }
        public async Task<int> Add(Product entity)
        {
            string jsonCharacteristics = JsonSerializer.Serialize(entity.Characteristics);
            string jsonPhotos = JsonSerializer.Serialize(entity.Photos);

            string sql = @"INSERT INTO Products (Name, Price, Category, Characteristics, Discount, DiscountEnds, Photos)
                           VALUES (@Name, @Price, @Category, @Characteristics, @Discount, @DiscountEnds, @Photos);";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                int affectedRows = await connection.ExecuteAsync(sql, new
                {
                    Name = entity.Name, Price = entity.Price, Category = entity.Category, Characteristics = jsonCharacteristics,
                            Discount = entity.Discount, DiscountEnds = entity.DiscountEnds, Photos = jsonPhotos
                });
                return affectedRows;
            }
        }

        public async Task<Product> Get(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            string sql = @"Select * FROM Products;";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var result = await connection.QueryAsync<ProductDTO>(sql);
                var newResult = new List<Product>();

                foreach (var prod in result)
                {
                    var newProd = new Product
                    {
                        Id = prod.Id,
                        Name = prod.Name,
                        Price = prod.Price,
                        Category = prod.Category,
                        Characteristics = JsonSerializer.Deserialize<string[]>(prod.Characteristics),
                        Discount = prod.Discount,
                        DiscountEnds = prod.DiscountEnds,
                        Photos = JsonSerializer.Deserialize<string[]>(prod.Photos)
                    };
                    newResult.Add(newProd);
                }
                return newResult;
            }
        }

        public async Task<int> Remove(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<int> Update(Client client)
        {
            throw new System.NotImplementedException();
        }
    }
}
