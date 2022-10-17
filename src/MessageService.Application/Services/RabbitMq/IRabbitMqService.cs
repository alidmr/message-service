using RabbitMQ.Client;

namespace MessageService.Application.Services.RabbitMq
{
    public interface IRabbitMqService : IDisposable
    {
        IModel Connect();
        void Publish<T>(T @event);
    }
}