using MessageService.Application.Base;
using MessageService.Application.Features.Users.GetUserMessagesByUserName.Dtos;

namespace MessageService.Application.Features.Users.GetUserMessagesByUserName.Queries
{
    public class GetUserMessagesByUserNameQueryResult :BaseResult
    {
        public List<GetUserMessagesByUserNameDto> Result { get; set; }
    }
}