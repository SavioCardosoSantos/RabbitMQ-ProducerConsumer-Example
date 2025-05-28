namespace ApiRabbitMQ.Models.DTOs.Product
{
    public class ProductRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Stock { get; set; }
    }
}
