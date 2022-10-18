using MediatR;
using MessageService.Application.Features.Accounts.Login.Commands;
using MessageService.Application.Features.Accounts.Register.Commands;
using Microsoft.AspNetCore.Mvc;

namespace MessageService.Api.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("register")]
        public async Task<RegisterCommandResult> Register([FromBody] RegisterCommand command)
        {
            var result = await _mediator.Send(command);
            return result;
        }

        [HttpPost]
        [Route("login")]
        public async Task<LoginCommandResult> Login([FromBody] LoginCommand command)
        {
            var result = await _mediator.Send(command);
            return result;
        }
    }
}