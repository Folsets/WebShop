using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebShop.Data.Entities;

namespace WebShop.Data.Interfaces
{
    public interface IGenericRepository<T, in TId> where T : class

    {
        Task<int> Add(T entity);

        Task<T> Get(TId id);

        Task<IEnumerable<T>> GetAll();

        Task<int> Remove(TId id);

        Task<int> Update(Client client);
    }
}
