using Microsoft.EntityFrameworkCore;
using RefactorThis.DataContext;
using RefactorThis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RefactorThis.Repository
{
    public class ProductsRepository<TContext> : IProductsRepository<Product>
        where TContext : ApplicationDBContext
    {
        readonly ApplicationDBContext _context;

        public ProductsRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task Add(Product product)
        {
            product.Id = new Guid();
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<Product> Get(Guid id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetAll(string name)
        {
            return await _context.Products.Where(p => p.Name == name).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<bool> Exists(Guid id)
        {
            return await _context.Products.AnyAsync(e => e.Id == id);
        }

        public async Task Update(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new System.Exception("An error occurred");
            }
        }
    }
}
