using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MessageService.Infrastructure.Context
{
    public class MessageServiceContext : IMessageServiceContext
    {
        private IMongoDatabase Db { get; set; }
        private IMongoClient MongoClient { get; set; }

        public MessageServiceContext(IOptions<MongoDbSettings> settings)
        {
            MongoClient = new MongoClient(settings.Value.Connection);
            Db = MongoClient.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return string.IsNullOrEmpty(name) ? null : Db.GetCollection<T>(name);
        }
    }
}