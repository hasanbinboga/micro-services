﻿using Catalog.Api.Data;
using Catalog.Api.Entities;
using MongoDB.Driver;

namespace Catalog.Api.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _context;
        public ProductRepository(ICatalogContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context
                            .Products
                            .Find(s => true)
                            .ToListAsync();
        }
        public async Task<Product> GetProduct(string id)
        {
            return await _context
                            .Products
                            .Find(s => s.Id == id)
                            .FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.ElemMatch(s => s.Name, name);

            return await _context
                           .Products
                           .Find(filter)
                           .ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
        {
            var filter = Builders<Product>.Filter.Eq(s => s.Category, categoryName);

            return await _context
                            .Products
                            .Find(filter)
                            .ToListAsync();
        }

        public async Task CreateProduct(Product product)
        {
            await _context.Products.InsertOneAsync(product);
        }
        public async Task<bool> UpdateProduct(Product product)
        {
            var updateResult = await _context
                                        .Products
                                        .ReplaceOneAsync(filter: s=>s.Id == product.Id, replacement: product);

            return updateResult.IsAcknowledged && updateResult.IsModifiedCountAvailable && updateResult.ModifiedCount == 1;
        }

        public async Task<bool> DeleteProduct(string id)
        {
            var filter = Builders<Product>.Filter.Eq(s=>s.Id, id);

            var deleteResult = await _context
                                       .Products       
                                       .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount == 1;
        }





    }
}
