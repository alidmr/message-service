using MessageService.Application.Base;
using MessageService.Domain.ValueObjects;

namespace MessageService.Application.Features.Accounts.Login.Commands
{
    public class LoginCommandResult : BaseResult
    {
        public AccessToken Token { get; set; }
    }
}