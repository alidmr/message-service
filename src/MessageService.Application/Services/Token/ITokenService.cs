using MessageService.Domain.Entities;
using MessageService.Infrastructure.Dtos;

namespace MessageService.Application.Services.Token
{
    public interface ITokenService
    {
        TokenDto CreateToken(User user);
    }
}