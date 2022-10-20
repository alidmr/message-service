using System.Net.Sockets;
using System.Text;
using MessageService.Infrastructure.Constants;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace MessageService.Infrastructure.Services.RabbitMq
{
    public class RabbitMqService : IRabbitMqService
    {
        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channel;
        private readonly ILogger<RabbitMqService> _logger;
        private readonly int _retryCount = 5;
        private bool _disposed;

        public RabbitMqService(ConnectionFactory connectionFactory, ILogger<RabbitMqService> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }
       
        private bool IsConnected => _connection != null && _connection.IsOpen && !_disposed;
        private bool IsChannel => _channel != null && _channel.IsOpen;
        
        public IModel GetChannel()
        {
            if (!IsConnected)
            {
                Connect();
            }

            if (IsChannel)
            {
                return _channel;
            }

            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: RabbitMqConstants.ExchangeName, type: "direct", durable: true, autoDelete: false);

            _logger.LogInformation("RabbitMq create channel...");
            return _channel;
        }

         private bool Connect()
        {
            _logger.LogInformation("RabbitMq connecting...");

            var policy = RetryPolicy.Handle<SocketException>()
                .Or<BrokerUnreachableException>()
                .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    _logger.LogError(ex, "RabbitMq not connect after {TimeOut}s ({ExceptionMessage})", $"{time.TotalSeconds:n1}", ex);
                });

            policy.Execute(() =>
            {
                _connection = _connectionFactory.CreateConnection();
                _logger.LogInformation("RabbitMq connected...");
            });

            if (IsConnected)
            {
                _connection.ConnectionShutdown += OnConnectionShutDown;
                _connection.CallbackException += OnCallbackException;
                _connection.ConnectionBlocked += OnConnectionBlocked;

                _logger.LogInformation("Success. RabbitMq connected to '{HostName}'", _connection.Endpoint.HostName);
                return true;
            }

            _logger.LogError("Error. RabbitMq not connection");
            return false;
        }

        public void Publish<T>(T @event, string queueName, string routingKey)
        {
            _channel = GetChannel();

            var bodyString = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(bodyString);

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;

            _channel.BasicPublish(exchange: RabbitMqConstants.ExchangeName, routingKey: routingKey, basicProperties: properties, body: body);
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            _channel?.Close();
            _channel?.Dispose();

            _connection?.Close();
            _connection?.Dispose();

            _logger.LogInformation("RabbitMqConnectionService --> RabbitMq connection disposed...");
        }

        private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs args)
        {
            if (_disposed)
                return;

            _logger.LogError("RabbitMq connection blocked..");

            Connect();
        }

        private void OnCallbackException(object sender, CallbackExceptionEventArgs args)
        {
            if (_disposed)
                return;

            _logger.LogError("RabbitMq connection callback exception...");

            Connect();
        }

        private void OnConnectionShutDown(object sender, ShutdownEventArgs reason)
        {
            if (_disposed)
                return;

            _logger.LogWarning("RabbitMq connection shutdown...");

            Connect();
        }
    }
}