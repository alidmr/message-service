using System.Net.Http.Json;
using System.Text;
using MessageService.Application.Constants;
using MessageService.Application.Events.Message;
using MessageService.Application.Services.RabbitMq;
using MessageService.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MessageService.Application.Services.Message
{
    public class MessageService : BackgroundService
    {
        private readonly IRabbitMqService _rabbitMqService;
        private readonly ILogger<MessageService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private IModel _channel;

        public MessageService(IRabbitMqService rabbitMqService, ILogger<MessageService> logger, IServiceProvider serviceProvider)
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
            var messageRepository = scope.ServiceProvider.GetRequiredService<IMessageRepository>();
            var blockUserRepostiroy = scope.ServiceProvider.GetRequiredService<IBlockUserRepository>();

            _channel.QueueDeclare(queue: RabbitMqConstants.MessageQueueName, durable: true, exclusive: false, autoDelete: false, null);

            _channel.QueueBind(queue: RabbitMqConstants.MessageQueueName, exchange: RabbitMqConstants.ExchangeName, routingKey: RabbitMqConstants.MessageRoutingKey);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            _channel.BasicConsume(queue: RabbitMqConstants.MessageQueueName, autoAck: false, consumer);

            consumer.Received += async (sender, @event) =>
            {
                var messageEvent = JsonConvert.DeserializeObject<MessageCreatedEvent>(Encoding.UTF8.GetString(@event.Body.ToArray()));
                if (messageEvent != null)
                {
                    var blockUser = await blockUserRepostiroy.GetAsync(x => x.Blocking == messageEvent.ReceiverUserName && x.Blocked == messageEvent.SenderUserName);
                    if (blockUser == null)
                    {
                        var message = Domain.Entities.Message.Create(messageEvent.Sender, messageEvent.SenderUserName, messageEvent.Receiver, messageEvent.ReceiverUserName, messageEvent.Content);
                        await messageRepository.AddAsync(message);

                        _logger.LogInformation($"send message. sender : {messageEvent.Sender} receiver : {messageEvent.Receiver}");
                    }
                    else
                        _logger.LogInformation($"message not send. receiver({messageEvent.Receiver}) blocked sender({messageEvent.Sender})");
                }

                _channel.BasicAck(@event.DeliveryTag, false);
            };
            _logger.LogInformation("Message consumed...");

            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _rabbitMqService.Dispose();
            return base.StopAsync(cancellationToken);
        }
    }
}