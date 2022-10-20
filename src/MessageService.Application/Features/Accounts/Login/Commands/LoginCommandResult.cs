using MessageService.Application.Base;
using MessageService.Infrastructure.Dtos;

namespace MessageService.Application.Features.Accounts.Login.Commands
{
    public class LoginCommandResult : BaseResult
    {
        public TokenDto Token { get; set; }
    }
}