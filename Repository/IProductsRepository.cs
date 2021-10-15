using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RefactorThis.Repository
{
    public interface IProductsRepository<TEntity> : IRepository<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> Get(Guid id);
        Task<IEnumerable<TEntity>> GetAll(string name);
    }
}
