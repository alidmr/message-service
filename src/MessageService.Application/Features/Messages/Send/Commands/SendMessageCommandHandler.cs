using MediatR;
using MessageService.Application.Constants;
using MessageService.Application.Events.Message;
using MessageService.Application.Features.Messages.Send.Validator;
using MessageService.Domain.Repositories;
using MessageService.Infrastructure.Constants;
using MessageService.Infrastructure.Dtos;
using MessageService.Infrastructure.Services.RabbitMq;

namespace MessageService.Application.Features.Messages.Send.Commands
{
    public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, SendMessageCommandResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRabbitMqService _rabbitMqService;

        public SendMessageCommandHandler(IUserRepository userRepository, IRabbitMqService rabbitMqService)
        {
            _userRepository = userRepository;
            _rabbitMqService = rabbitMqService;
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

            var messageEvent = new MessageCreatedEvent(request.Sender, request.Receiver, request.MessageContent, request.SenderUserName, request.ReceiverUserName);
            _rabbitMqService.Publish(messageEvent, RabbitMqConstants.MessageQueueName, RabbitMqConstants.MessageRoutingKey);

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