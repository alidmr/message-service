using System.Text.Json.Serialization;
using MediatR;

namespace MessageService.Application.Features.Users.BlockUsers.Command
{
    public class BlockUserCommand : IRequest<BlockUserCommandResult>
    {
        [JsonIgnore] 
        public string BlockingUserName { get; set; }
        public string BlockedUserName { get; set; }
    }
}