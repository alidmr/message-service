using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MessageService.Domain.Entities;
using MessageService.Domain.ValueObjects;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace MessageService.Application.Services.Token
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public AccessToken CreateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var tokenHandler = new JwtSecurityTokenHandler();

            var accessToken = new AccessToken
            {
                ExpirationDate = DateTime.Now.AddMinutes(10)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["Token:Issuer"],
                Audience = _configuration["Token:Audience"],
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.FirstName),
                    new Claim("UserName", user.UserName),
                    new Claim(ClaimTypes.Surname, user.LastName)
                }),
                Expires = accessToken.ExpirationDate,
                NotBefore = DateTime.Now,
                SigningCredentials = signingCredentials,
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            accessToken.Token = tokenHandler.WriteToken(token);
            return accessToken;
        }
    }
}