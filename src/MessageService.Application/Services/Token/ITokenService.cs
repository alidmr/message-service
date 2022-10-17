using MessageService.Domain.Entities;
using MessageService.Domain.ValueObjects;

namespace MessageService.Application.Services.Token
{
    public interface ITokenService
    {
        AccessToken CreateToken(User user);
    }
}