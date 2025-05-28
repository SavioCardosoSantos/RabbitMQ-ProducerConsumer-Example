using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using ApiRabbitMQ.RabbitMQ.Interfaces;

namespace ApiRabbitMQ.RabbitMQ
{
    public class RabbitMQProducer : IRabbitMQProducer
    {
        public async Task SendProductMessage<T>(T message)
        {
            // Definição do servidor Rabbit MQ (utilizando uma imagem Docker)
            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            // Criação da conexão(connection)
            var connection = await factory.CreateConnectionAsync();

            // Criação do canal(channel)
            using var channel = await connection.CreateChannelAsync();

            // Declaração da fila(queue), passando como parâmetro o nome da fila e a propriedade exclusive(ser acessível somente pela conexão que a criou) como false
            await channel.QueueDeclareAsync("product", exclusive: false);

            // Serializa a mensagem
            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            // Publica dados na fila "product"
            await channel.BasicPublishAsync(exchange: "", routingKey: "product", body: body);
        }
    }
}
