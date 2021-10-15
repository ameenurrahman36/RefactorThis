using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RefactorThis.Repository
{
    public interface IProductOptionsRepository<TEntity> : IRepository<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAll(Guid productId);
        Task<TEntity> Get(Guid productId, Guid id);
    }
}
