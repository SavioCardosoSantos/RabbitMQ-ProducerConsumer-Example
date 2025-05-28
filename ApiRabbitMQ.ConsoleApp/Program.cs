using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

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

// Criação do objeto EventConsumer, que vai ouvir as mensagens do channel enviado pelo producer
var consumer = new AsyncEventingBasicConsumer(channel);

consumer.ReceivedAsync += async (model, eventArgs) =>
{
    try
    {
        // Serializa a mensagem
        var body = eventArgs.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine($"[✓] Product message received: {message}");

        // Confirma manualmente que a mensagem foi processada com sucesso
        await channel.BasicNackAsync(deliveryTag: eventArgs.DeliveryTag, multiple: false, requeue: false, cancellationToken: default);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error receiving Product message: {ex.Message}");
    }
};

// Lê a mensagem
await channel.BasicConsumeAsync(queue: "product", autoAck: true, consumer: consumer);

Console.ReadKey();