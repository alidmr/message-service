using System.Text;
using MessageService.Application.Constants;
using MessageService.Application.Events.Users;
using MessageService.Application.Services.RabbitMq;
using MessageService.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MessageService.Application.Services.UserLog
{
    public class UserLogService : BackgroundService
    {
        private readonly IRabbitMqService _rabbitMqService;
        private readonly ILogger<UserLogService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private IModel _channel;

        public UserLogService(IRabbitMqService rabbitMqService, ILogger<UserLogService> logger, IServiceProvider serviceProvider)
        {
            _rabbitMqService = rabbitMqService;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _channel = _rabbitMqService.GetChannel();

            return base.StartAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var userLogRepository = scope.ServiceProvider.GetRequiredService<IUserLogRepository>();

            _channel.QueueDeclare(queue: RabbitMqConstants.UserLogQueueName, durable: true, exclusive: false, autoDelete: false, null);

            _channel.QueueBind(queue: RabbitMqConstants.UserLogQueueName, exchange: RabbitMqConstants.ExchangeName, routingKey: RabbitMqConstants.UserLogRoutingKey);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            _channel.BasicConsume(queue: RabbitMqConstants.UserLogQueueName, autoAck: false, consumer);

            consumer.Received += async (sender, @event) =>
            {
                var userLogEvent = JsonConvert.DeserializeObject<UserLogEvent>(Encoding.UTF8.GetString(@event.Body.ToArray()));
                if (userLogEvent != null)
                {
                    var userLog = Domain.Entities.UserLog.Create(userLogEvent.UserName, userLogEvent.Content);
                    await userLogRepository.AddAsync(userLog);
                }

                _channel.BasicAck(@event.DeliveryTag, false);
            };
            _logger.LogInformation("User Log consumed...");
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _rabbitMqService.Dispose();
            return base.StopAsync(cancellationToken);
        }
    }
}