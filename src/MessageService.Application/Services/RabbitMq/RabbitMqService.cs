using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace MessageService.Application.Services.RabbitMq
{
    public class RabbitMqService : IRabbitMqService
    {
        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channel;

        public static string ExchangeName = "MessageServiceExchange";
        public static string RoutingKeyUserLog = "user-log-route";
        public static string QueueName = "user-log";

        private readonly ILogger<RabbitMqService> _logger;

        public RabbitMqService(ConnectionFactory connectionFactory, ILogger<RabbitMqService> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public IModel Connect()
        {
            if (_channel != null && _channel.IsOpen)
            {
                return _channel;
            }

            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: ExchangeName, type: "direct", durable: true, autoDelete: false);

            _channel.QueueDeclare(queue: QueueName, durable: true, exclusive: false, autoDelete: false, null);

            _channel.QueueBind(queue: QueueName, exchange: ExchangeName, routingKey: RoutingKeyUserLog);

            _logger.LogInformation("RabbitMq connected...");
            return _channel;
        }

        public void Publish<T>(T @event)
        {
            _channel = Connect();

            var bodyString = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(bodyString);

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;

            _channel.BasicPublish(exchange: ExchangeName, routingKey: RoutingKeyUserLog, basicProperties: properties, body: body);
        }

        public void Dispose()
        {
            _channel?.Close();
            _channel?.Dispose();

            _connection?.Close();
            _connection?.Dispose();

            _logger.LogInformation("RabbitMq connection disposed...");
        }
    }
}