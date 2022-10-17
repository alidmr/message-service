using MediatR;

namespace MessageService.Application.Features.Accounts.Commands
{
    public class RegisterCommand : IRequest<RegisterCommandResult>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}