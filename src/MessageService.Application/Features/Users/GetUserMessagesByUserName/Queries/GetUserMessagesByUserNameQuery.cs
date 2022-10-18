using System.Text.Json.Serialization;
using MediatR;

namespace MessageService.Application.Features.Users.GetUserMessagesByUserName.Queries
{
    public class GetUserMessagesByUserNameQuery : IRequest<GetUserMessagesByUserNameQueryResult>
    {
        [JsonIgnore]
        public string SenderUserName { get; set; }
        public string ReceiverUserName { get; set; }
    }
}