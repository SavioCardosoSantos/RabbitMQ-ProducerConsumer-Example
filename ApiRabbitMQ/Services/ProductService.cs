using ApiRabbitMQ.Context;
using ApiRabbitMQ.Models.DTOs.Product;
using ApiRabbitMQ.Models.Entities;
using ApiRabbitMQ.RabbitMQ;
using ApiRabbitMQ.RabbitMQ.Interfaces;
using ApiRabbitMQ.Services.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace ApiRabbitMQ.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private IRabbitMQProducer _producer;
        public ProductService(AppDbContext dbContext, IRabbitMQProducer producer)
        {
            _context = dbContext;
            _producer = producer;
        }
        public async Task<IEnumerable<ProductResponse>> GetProductList()
        {
            var products = await _context.Products.ToListAsync();
            return products.Adapt<IEnumerable<ProductResponse>>();
        }
        public async Task<ProductResponse?> GetProductById(int id)
        {
            var product = await _context.Products.Where(x => x.Id == id).FirstOrDefaultAsync();
            return product.Adapt<ProductResponse>();
        }
        public async Task<ProductResponse> AddProduct(ProductRequest product)
        {
            var result = _context.Products.Add(product.Adapt<Product>());
            await _context.SaveChangesAsync();

            var productResponse = result.Entity.Adapt<ProductResponse>();
            await _producer.SendProductMessage(productResponse);
            return productResponse;
        }
        public async Task<ProductResponse> UpdateProduct(int productId, ProductRequest product)
        {
            var productEntity = product.Adapt<Product>();
            productEntity.Id = productId;

            var result = _context.Products.Update(productEntity);
            await _context.SaveChangesAsync();

            return result.Entity.Adapt<ProductResponse>();
        }
        public async Task<bool> DeleteProduct(int Id)
        {
            var filteredData = await _context.Products.Where(x => x.Id == Id).FirstOrDefaultAsync();
            var success = false;
            if (filteredData != null)
            {
                var result = _context.Remove(filteredData);
                await _context.SaveChangesAsync();
                success = result != null;
            }

            return success;
        }
    }
}
