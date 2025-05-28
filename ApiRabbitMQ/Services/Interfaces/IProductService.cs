using ApiRabbitMQ.Models.DTOs.Product;
using ApiRabbitMQ.Models.Entities;

namespace ApiRabbitMQ.Services.Interfaces
{
    public interface IProductService
    {
        public Task<IEnumerable<ProductResponse>> GetProductList();
        public Task<ProductResponse?> GetProductById(int id);
        public Task<ProductResponse> AddProduct(ProductRequest product);
        public Task<ProductResponse> UpdateProduct(int productId, ProductRequest product);
        public Task<bool> DeleteProduct(int Id);
    }
}
