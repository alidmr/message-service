using MessageService.Domain.Entities;
using MessageService.Domain.Repositories;
using MessageService.Infrastructure.Context;
using MessageService.Infrastructure.Repositories.Base;

namespace MessageService.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(IMessageServiceContext context) : base(context)
        {
        }
    }
}