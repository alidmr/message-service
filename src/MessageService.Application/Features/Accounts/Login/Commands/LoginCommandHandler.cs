using MediatR;
using MessageService.Application.Constants;
using MessageService.Application.Events.Users;
using MessageService.Application.Features.Accounts.Login.Validator;
using MessageService.Application.Services.RabbitMq;
using MessageService.Application.Services.Token;
using MessageService.Domain.Repositories;
using MessageService.Infrastructure.Dtos;
using MessageService.Infrastructure.Helpers;

namespace MessageService.Application.Features.Accounts.Login.Commands
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginCommandResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IRabbitMqService _rabbitMqService;

        public LoginCommandHandler(IUserRepository userRepository, ITokenService tokenService, IRabbitMqService rabbitMqService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _rabbitMqService = rabbitMqService;
        }

        public async Task<LoginCommandResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var validation = await new LoginCommandValidator().ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
            {
                return new LoginCommandResult()
                {
                    Success = false,
                    Messages = validation.Errors.Select(x => new MessageDto()
                    {
                        Message = x.ErrorMessage
                    }).ToList()
                };
            }

            var user = await _userRepository.GetAsync(x => x.UserName == request.UserName);
            if (user == null)
            {
                return new LoginCommandResult()
                {
                    Success = false,
                    Messages = new List<MessageDto>()
                    {
                        new MessageDto() {Message = ApplicationErrorMessage.ApplicationError8}
                    }
                };
            }

            var password = HashingHelper.HashPassword(request.Password, user.PasswordSalt);
            if (password != user.Password)
            {
                CreateUserLog(request.UserName, "Giriş başarısız. Şifre hatalı");
                return new LoginCommandResult()
                {
                    Success = false,
                    Messages = new List<MessageDto>()
                    {
                        new MessageDto() {Message = ApplicationErrorMessage.ApplicationError8}
                    }
                };
            }

            var token = _tokenService.CreateToken(user);
            user.SetToken(token);
            await _userRepository.UpdateAsync(user.Id, user);

            CreateUserLog(request.UserName, "Giriş başarlı");

            return new LoginCommandResult()
            {
                Success = true,
                Messages = new List<MessageDto>()
                {
                    new MessageDto()
                    {
                        Message = "İşlem başarılı"
                    }
                },
                Token = token
            };
        }

        private void CreateUserLog(string userName, string content)
        {
            var userLogEvent = new UserLogEvent(userName, content);
            _rabbitMqService.Publish(userLogEvent, RabbitMqConstants.UserLogQueueName, RabbitMqConstants.UserLogRoutingKey);
        }
    }
}