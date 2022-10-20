using MessageService.Domain.Entities;
using MessageService.Domain.Repositories;
using MessageService.Infrastructure.Context;
using MessageService.Infrastructure.Repositories.Base;

namespace MessageService.Infrastructure.Repositories
{
    public class UserLogRepository : Repository<UserLog>, IUserLogRepository
    {
        public UserLogRepository(IMessageServiceContext context) : base(context, "UserLogs")
        {
        }
    }
}