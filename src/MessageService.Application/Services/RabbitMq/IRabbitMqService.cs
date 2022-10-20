using RabbitMQ.Client;

namespace MessageService.Application.Services.RabbitMq
{
    public interface IRabbitMqService : IDisposable
    {
        IModel GetChannel();
        void Publish<T>(T @event, string queueName, string routingKey);
    }
}