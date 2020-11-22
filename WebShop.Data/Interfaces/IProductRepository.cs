using System.Collections.Generic;
using System.Threading.Tasks;
using WebShop.Data.Entities;

namespace WebShop.Data.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product, int>
    {
    }
}
