using MediatR;

namespace MessageService.Application.Features.Users.BlockUsers.Queries
{
    public class GetBlockedUsersQuery : IRequest<GetBlockedUsersQueryResult>
    {
        public string BlockingUserName { get; set; }
    }
}