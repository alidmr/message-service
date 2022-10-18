using MediatR;
using MessageService.Api.Auth;
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
        private readonly IApplicationUser _applicationUser;

        public MessageController(IMediator mediator, IApplicationUser applicationUser)
        {
            _mediator = mediator;
            _applicationUser = applicationUser;
        }

        [HttpPost]
        public async Task<SendMessageCommandResult> SendMessage([FromBody] SendMessageCommand command)
        {
            command.Sender = $"{_applicationUser.FirstName} {_applicationUser.LastName}";
            command.SenderUserName = _applicationUser.UserName;
            var result = await _mediator.Send(command);
            return result;
        }
    }
}