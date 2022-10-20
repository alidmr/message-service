using RabbitMQ.Client;

namespace MessageService.Infrastructure.Services.RabbitMq
{
    public interface IRabbitMqService : IDisposable
    {
        IModel GetChannel();
        void Publish<T>(T @event, string queueName, string routingKey);
    }
}