using Microsoft.EntityFrameworkCore;
using RefactorThis.DataContext;
using RefactorThis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RefactorThis.Repository
{
    public class ProductOptionsRepository<TContext> : IProductOptionsRepository<ProductOption>
        where TContext : ApplicationDBContext
    {
        readonly ApplicationDBContext _context;

        public ProductOptionsRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task Add(ProductOption productOption)
        {
            productOption.Id = new Guid();
            _context.ProductOptions.Add(productOption);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(ProductOption productOption)
        {
            _context.ProductOptions.Remove(productOption);
            await _context.SaveChangesAsync();
        }

        public async Task<ProductOption> Get(Guid productId, Guid id)
        {
            return await _context.ProductOptions
                .Select(x => new ProductOption
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                })
                .FirstOrDefaultAsync(p => p.Id == id && p.ProductId == productId);
        }

        public async Task<IEnumerable<ProductOption>> GetAll(Guid productId)
        {
            return await _context.ProductOptions
                .Where(p => p.ProductId == productId)
                .Select(x => new ProductOption
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                })
                .ToListAsync();
        }

        public async Task<bool> Exists(Guid id)
        {
            return await _context.ProductOptions.AnyAsync(e => e.Id == id);
        }

        public async Task Update(ProductOption productOption)
        {
            _context.Entry(productOption).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }
    }
}
