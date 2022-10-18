using MediatR;

namespace MessageService.Application.Features.Users.GetUserMessages.Queries
{
    public class GetUserMessagesQuery : IRequest<GetUserMessagesQueryResult>
    {
        public string UserName { get; set; }
    }
}