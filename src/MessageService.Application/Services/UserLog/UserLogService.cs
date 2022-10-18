using System.Text;
using MessageService.Application.Constants;
using MessageService.Application.Events;
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
            _channel = _rabbitMqService.Connect();

            return base.StartAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var userLogRepository = scope.ServiceProvider.GetRequiredService<IUserLogRepository>();

            var consumer = new AsyncEventingBasicConsumer(_channel);
            _channel.BasicConsume(queue: RabbitMqConstants.UserLogQueueName, autoAck: false, consumer);

            consumer.Received += async (sender, @event) =>
            {
                var userLogEvent = JsonConvert.DeserializeObject<UserLogEvent>(Encoding.UTF8.GetString(@event.Body.ToArray()));

                if (userLogEvent != null)
                {
                    var userLog = Domain.Entities.UserLog.Create(userLogEvent.UserName, userLogEvent.Content);

                    await userLogRepository.AddAsync(userLog);

                    _logger.LogInformation("User Log consume...");
                }

                _channel.BasicAck(@event.DeliveryTag, false);
            };

            return Task.CompletedTask;
        }

        private async Task ConsumerOnReceived(object sender, BasicDeliverEventArgs @event)
        {
            var userLogEvent = JsonConvert.DeserializeObject<UserLogEvent>(Encoding.UTF8.GetString(@event.Body.ToArray()));

            if (userLogEvent != null)
            {
                var userLog = Domain.Entities.UserLog.Create(userLogEvent.UserName, userLogEvent.Content);

                //await _userLogRepository.AddAsync(userLog);

                _logger.LogInformation("User Log consume...");
            }

            _channel.BasicAck(@event.DeliveryTag, false);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _rabbitMqService.Dispose();
            return base.StopAsync(cancellationToken);
        }
    }
}