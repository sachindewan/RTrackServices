using Catalog.API.Data.Interfaces;
using Catalog.API.Entities;
using Catalog.API.Repositories.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _context;

        public ProductRepository(ICatalogContext catalogContext)
        {
            _context = catalogContext;
        
        }

        public async Task<Product> GetProduct(string id)
        {
            return await _context
                            .Products
                            .Find(p => p.Id == id)
                            .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
        {

            return await _context
                            .Products
                            .Find(p => p.Category == categoryName)
                            .ToListAsync();
  
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            return await _context
                             .Products
                             .Find(p => p.Name == name)
                             .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context
                            .Products
                            .Find(p => true)
                            .ToListAsync();
        }

        public async Task Create(Product product)
        {
            await _context.Products.InsertOneAsync(product);
        }

        public async Task<bool> Delete(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(m => m.Id, id);
            DeleteResult deleteResult = await _context
                                                       .Products
                                                       .DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }

        
        public async Task<bool> Update(Product product)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(m => m.Id, product.Id);
            var updateResult = await _context
                                            .Products
                                            .ReplaceOneAsync(filter,product);
            return updateResult.IsAcknowledged
                && updateResult.ModifiedCount > 0;
        }
    }
}
