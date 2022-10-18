using System.Text.Json.Serialization;
using MediatR;

namespace MessageService.Application.Features.Messages.Send.Commands
{
    public class SendMessageCommand : IRequest<SendMessageCommandResult>
    {
        [JsonIgnore] 
        public string Sender { get; set; }
        
        [JsonIgnore] 
        public string SenderUserName { get; set; }
        public string Receiver { get; set; }
        public string ReceiverUserName { get; set; }
        public string MessageContent { get; set; }
    }
}