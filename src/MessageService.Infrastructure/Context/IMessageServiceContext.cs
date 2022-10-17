using MongoDB.Driver;

namespace MessageService.Infrastructure.Context
{
    public interface IMessageServiceContext
    {
        IMongoCollection<T> GetCollection<T>(string name);
    }
}