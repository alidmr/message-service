using RabbitMQ.Client;

namespace MessageService.Application.Services.RabbitMq
{
    public interface IRabbitMqService : IDisposable
    {
        IModel Connect(string queueName, string routingKey);
        void Publish<T>(T @event, string queueName, string routingKey);
    }
}