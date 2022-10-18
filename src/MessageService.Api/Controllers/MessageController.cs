using MediatR;
using MessageService.Application.Features.Messages.Send.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessageService.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/messages")]
    public class MessageController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MessageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("send-message")]
        public async Task<SendMessageCommandResult> SendMessage([FromBody] SendMessageCommand command)
        {
            // todo: login olan kullanıcının user name i set edilecek
            command.Sender = "";
            var result = await _mediator.Send(command);
            return result;
        }
    }
}