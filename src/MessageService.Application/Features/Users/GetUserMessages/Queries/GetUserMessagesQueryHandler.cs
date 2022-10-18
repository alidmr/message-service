using MediatR;
using MessageService.Application.Constants;
using MessageService.Application.Features.Users.GetUserMessages.Dtos;
using MessageService.Application.Features.Users.GetUserMessages.Validator;
using MessageService.Domain.Repositories;
using MessageService.Infrastructure.Dtos;

namespace MessageService.Application.Features.Users.GetUserMessages.Queries
{
    public class GetUserMessagesQueryHandler : IRequestHandler<GetUserMessagesQuery, GetUserMessagesQueryResult>
    {
        private readonly IMessageRepository _messageRepository;

        public GetUserMessagesQueryHandler(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<GetUserMessagesQueryResult> Handle(GetUserMessagesQuery request, CancellationToken cancellationToken)
        {
            var validation = await new GetUserMessagesQueryValidator().ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
            {
                return new GetUserMessagesQueryResult()
                {
                    Success = false,
                    Messages = validation.Errors.Select(x => new MessageDto()
                    {
                        Message = x.ErrorMessage
                    }).ToList()
                };
            }

            var messages = await _messageRepository.GetAllAsync(x => x.SenderUserName == request.UserName);
            if (!messages.Any())
            {
                return new GetUserMessagesQueryResult()
                {
                    Success = false,
                    Messages = new List<MessageDto>()
                    {
                        new MessageDto() {Message = ApplicationErrorMessage.ApplicationError11}
                    }
                };
            }

            return new GetUserMessagesQueryResult()
            {
                Success = true,
                Result = messages.Select(x => new GetUserMessagesDto()
                {
                    Content = x.Content,
                    Receiver = x.Receiver,
                    ReceiverUserName = x.Receiver
                }).ToList()
            };
        }
    }
}