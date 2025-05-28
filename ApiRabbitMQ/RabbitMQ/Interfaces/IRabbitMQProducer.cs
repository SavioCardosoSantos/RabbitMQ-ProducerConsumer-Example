namespace ApiRabbitMQ.RabbitMQ.Interfaces
{
    public interface IRabbitMQProducer
    {
        Task SendProductMessage<T>(T message);
    }
}
