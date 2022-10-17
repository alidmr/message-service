using MediatR;
using MessageService.Application.Constants;
using MessageService.Domain.Entities;
using MessageService.Domain.Repositories;
using MessageService.Infrastructure.Dtos;
using MessageService.Infrastructure.Helpers;

namespace MessageService.Application.Features.Accounts.Commands
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterCommandResult>
    {
        private readonly IUserRepository _userRepository;

        public RegisterCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<RegisterCommandResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var validation = await new RegisterCommandValidator().ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
            {
                return new RegisterCommandResult()
                {
                    Success = false,
                    Messages = validation.Errors.Select(x => new MessageDto()
                    {
                        Message = x.ErrorMessage
                    }).ToList()
                };
            }

            var existsUser = await _userRepository.GetAsync(x => x.UserName == request.UserName);
            if (existsUser != null)
            {
                return new RegisterCommandResult()
                {
                    Success = false,
                    Messages = new List<MessageDto>()
                    {
                        new MessageDto()
                        {
                            Message = ApplicationErrorMessage.ApplicationError6
                        }
                    }
                };
            }

            var salt = HashingHelper.GetSalt();
            var password = HashingHelper.HashPassword(request.Password, salt);

            var user = User.Create(request.FirstName, request.LastName, request.UserName);
            user.SetPassword(password, salt);

            await _userRepository.AddAsync(user);

            return new RegisterCommandResult()
            {
                Success = true,
                Messages = new List<MessageDto>()
                {
                    new MessageDto()
                    {
                        Message = "İşlem başarılı"
                    }
                }
            };
        }
    }
}