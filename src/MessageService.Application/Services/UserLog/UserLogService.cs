using System.Text;
using MediatR;
using MessageService.Application.Events.Users;
using MessageService.Infrastructure.Constants;
using MessageService.Infrastructure.Services.RabbitMq;
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
        private IModel _channel;
        private readonly IMediator _mediator;

        public UserLogService(IRabbitMqService rabbitMqService, ILogger<UserLogService> logger, IMediator mediator)
        {
            _rabbitMqService = rabbitMqService;
            _logger = logger;
            _mediator = mediator;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _channel = _rabbitMqService.GetChannel();

            return base.StartAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _channel.QueueDeclare(queue: RabbitMqConstants.UserLogQueueName, durable: true, exclusive: false, autoDelete: false, null);

            _channel.QueueBind(queue: RabbitMqConstants.UserLogQueueName, exchange: RabbitMqConstants.ExchangeName, routingKey: RabbitMqConstants.UserLogRoutingKey);

            _channel.BasicQos(0, 50, false);

            // var consumer = new AsyncEventingBasicConsumer(_channel);
            var consumer = new EventingBasicConsumer(_channel);
            _channel.BasicConsume(queue: RabbitMqConstants.UserLogQueueName, autoAck: false, consumer);

            try
            {
                consumer.Received += (sender, @event) =>
                {
                    try
                    {
                        var userLogEvent = JsonConvert.DeserializeObject<UserLogEvent>(Encoding.UTF8.GetString(@event.Body.ToArray()));
                        if (userLogEvent != null)
                        {
                            _mediator.Publish(userLogEvent, stoppingToken).GetAwaiter().GetResult();
                            // _mediator.Send(userLogEvent, stoppingToken).GetAwaiter().GetResult();
                        }

                        _channel.BasicAck(@event.DeliveryTag, false);
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                        throw new Exception(exception.ToString());
                    }
                };
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

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