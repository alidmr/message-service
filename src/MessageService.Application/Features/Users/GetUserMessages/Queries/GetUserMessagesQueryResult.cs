using MessageService.Application.Base;
using MessageService.Application.Features.Users.GetUserMessages.Dtos;

namespace MessageService.Application.Features.Users.GetUserMessages.Queries
{
    public class GetUserMessagesQueryResult : BaseResult
    {
        public List<GetUserMessagesDto> Result { get; set; }
    }
}