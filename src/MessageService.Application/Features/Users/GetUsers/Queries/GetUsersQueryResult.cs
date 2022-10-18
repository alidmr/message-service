using MessageService.Application.Base;
using MessageService.Application.Features.Users.GetUsers.Dtos;

namespace MessageService.Application.Features.Users.GetUsers.Queries
{
    public class GetUsersQueryResult : BaseResult
    {
        public List<UserDto> Result { get; set; }
    }
}