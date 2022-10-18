using MessageService.Application.Base;
using MessageService.Application.Features.Users.BlockUsers.Dtos;

namespace MessageService.Application.Features.Users.BlockUsers.Queries
{
    public class GetBlockedUsersQueryResult : BaseResult
    {
        public List<GetBlockedUsersDto> Result { get; set; }
    }
}