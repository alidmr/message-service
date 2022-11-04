using System.Text;
using MediatR;
using MessageService.Application.Events.Users;
using MessageService.Domain.Repositories;
using MessageService.Infrastructure.Constants;
using MessageService.Infrastructure.Services.RabbitMq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MessageService.Application.Services.UserLog
{
    public class UserLogTestService
    {
        private readonly IRabbitMqService _rabbitMqService;
        private readonly ILogger<UserLogService> _logger;
        private IModel _channel;
        private readonly IMediator _mediator;

        public UserLogTestService(IRabbitMqService rabbitMqService, ILogger<UserLogService> logger, IMediator mediator)
        {
            _rabbitMqService = rabbitMqService;
            _logger = logger;
            _mediator = mediator;
        }

        public Task Consume()
        {
            _channel = _rabbitMqService.GetChannel();
            _channel.QueueDeclare(queue: RabbitMqConstants.UserLogQueueName, durable: true, exclusive: false, autoDelete: false, null);
            _channel.QueueBind(queue: RabbitMqConstants.UserLogQueueName, exchange: RabbitMqConstants.ExchangeName, routingKey: RabbitMqConstants.UserLogRoutingKey);
            _channel.BasicQos(0, 10, false);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            _channel.BasicConsume(queue: RabbitMqConstants.UserLogQueueName, autoAck: false, consumer);

            consumer.Received += async (sender, @event) =>
            {
                var userLogEvent = JsonConvert.DeserializeObject<UserLogEvent>(Encoding.UTF8.GetString(@event.Body.ToArray()));
                if (userLogEvent != null)
                {
                    try
                    {
                        await _mediator.Publish(userLogEvent);
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                        throw;
                    }
                }

                _channel.BasicAck(@event.DeliveryTag, false);
            };

            _logger.LogInformation("User Log consumed...");

            return Task.CompletedTask;
        }

        public void Disconnect()
        {
            _rabbitMqService.Dispose();
        }
    }
}