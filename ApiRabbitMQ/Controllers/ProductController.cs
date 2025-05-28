using ApiRabbitMQ.Models.DTOs.Product;
using ApiRabbitMQ.Models.Entities;
using ApiRabbitMQ.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiRabbitMQ.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> GetAll()
        {
            var products = await _service.GetProductList();
            return Ok(products);
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> GetById(int productId)
        {
            var product = await _service.GetProductById(productId);
            if (product == null)
                return NotFound("Product not found.");

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult> Add(ProductRequest product)
        {
            var productObj = await _service.AddProduct(product);
            if (productObj == null)
                return BadRequest();

            return Ok(productObj);
        }

        [HttpPut("{productId}")]
        public async Task<ActionResult> Update(int productId, ProductRequest product)
        {
            var productObj = await _service.UpdateProduct(productId, product);
            if (productObj == null)
                return BadRequest();

            return Ok(productObj);
        }

        [HttpDelete("{productId}")]
        public async Task<ActionResult> Delete(int productId)
        {
            var success = await _service.DeleteProduct(productId);
            if (!success)
                return NotFound("Product not found.");

            return Ok("Product deleted.");
        }
    }
}
