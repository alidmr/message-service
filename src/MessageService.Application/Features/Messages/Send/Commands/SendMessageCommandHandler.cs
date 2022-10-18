using MediatR;
using MessageService.Application.Constants;
using MessageService.Application.Features.Messages.Send.Validator;
using MessageService.Domain.Entities;
using MessageService.Domain.Repositories;
using MessageService.Infrastructure.Dtos;

namespace MessageService.Application.Features.Messages.Send.Commands
{
    public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, SendMessageCommandResult>
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUserRepository _userRepository;

        public SendMessageCommandHandler(IMessageRepository messageRepository, IUserRepository userRepository)
        {
            _messageRepository = messageRepository;
            _userRepository = userRepository;
        }

        public async Task<SendMessageCommandResult> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            var validation = await new SendMessageCommandValidator().ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
            {
                return new SendMessageCommandResult()
                {
                    Success = false,
                    Messages = validation.Errors.Select(x => new MessageDto()
                    {
                        Message = x.ErrorMessage
                    }).ToList()
                };
            }

            var receiverUser = await _userRepository.GetAsync(x => x.UserName == request.ReceiverUserName);
            if (receiverUser == null)
            {
                return new SendMessageCommandResult()
                {
                    Success = false,
                    Messages = new List<MessageDto>()
                    {
                        new() {Message = ApplicationErrorMessage.ApplicationError7}
                    }
                };
            }

            var message = Message.Create(request.Sender, request.Receiver, request.MessageContent);
            await _messageRepository.AddAsync(message);

            return new SendMessageCommandResult()
            {
                Success = true,
                Messages = new List<MessageDto>()
                {
                    new() {Message = "İşlem başarılı"}
                }
            };
        }
    }
}