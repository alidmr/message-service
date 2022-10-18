using MediatR;
using MessageService.Application.Features.Users.BlockUsers.Dtos;
using MessageService.Domain.Repositories;

namespace MessageService.Application.Features.Users.BlockUsers.Queries
{
    public class GetBlockedUsersQueryHandler : IRequestHandler<GetBlockedUsersQuery, GetBlockedUsersQueryResult>
    {
        private readonly IBlockUserRepository _blockUserRepository;

        public GetBlockedUsersQueryHandler(IBlockUserRepository blockUserRepository)
        {
            _blockUserRepository = blockUserRepository;
        }

        public async Task<GetBlockedUsersQueryResult> Handle(GetBlockedUsersQuery request, CancellationToken cancellationToken)
        {
            var blockedUsers = await _blockUserRepository.GetAllAsync(x => x.Blocking == request.BlockingUserName);

            return new GetBlockedUsersQueryResult()
            {
                Success = true,
                Result = blockedUsers.Select(x => new GetBlockedUsersDto()
                {
                    Blocked = x.Blocked
                }).ToList()
            };
        }
    }
}