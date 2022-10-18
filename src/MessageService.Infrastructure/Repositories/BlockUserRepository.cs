using MessageService.Domain.Entities;
using MessageService.Domain.Repositories;
using MessageService.Infrastructure.Context;
using MessageService.Infrastructure.Repositories.Base;

namespace MessageService.Infrastructure.Repositories
{
    public class BlockUserRepository : Repository<BlockUser>, IBlockUserRepository
    {
        public BlockUserRepository(IMessageServiceContext context) : base(context, "block-users")
        {
        }
    }
}