using MessageService.Domain.Entities;
using MessageService.Domain.Repositories;
using MessageService.Infrastructure.Context;
using MessageService.Infrastructure.Repositories.Base;

namespace MessageService.Infrastructure.Repositories
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        public MessageRepository(IMessageServiceContext context) : base(context, "messages")
        {
        }
    }
}