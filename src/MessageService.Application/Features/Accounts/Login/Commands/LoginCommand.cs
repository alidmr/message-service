using MediatR;

namespace MessageService.Application.Features.Accounts.Login.Commands
{
    public class LoginCommand : IRequest<LoginCommandResult>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}