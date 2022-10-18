using MediatR;
using MessageService.Application.Features.Users.GetUserMessagesByUserName.Dtos;
using MessageService.Application.Features.Users.GetUserMessagesByUserName.Validator;
using MessageService.Domain.Repositories;
using MessageService.Infrastructure.Dtos;

namespace MessageService.Application.Features.Users.GetUserMessagesByUserName.Queries
{
    public class GetUserMessagesByUserNameQueryHandler : IRequestHandler<GetUserMessagesByUserNameQuery, GetUserMessagesByUserNameQueryResult>
    {
        private readonly IMessageRepository _messageRepository;

        public GetUserMessagesByUserNameQueryHandler(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<GetUserMessagesByUserNameQueryResult> Handle(GetUserMessagesByUserNameQuery request, CancellationToken cancellationToken)
        {
            var validation = await new GetUserMessagesByUserNameQueryValidator().ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
            {
                return new GetUserMessagesByUserNameQueryResult()
                {
                    Success = false,
                    Messages = validation.Errors.Select(x => new MessageDto()
                    {
                        Message = x.ErrorMessage
                    }).ToList()
                };
            }

            var messages = await _messageRepository.GetAllAsync(x => x.SenderUserName == request.SenderUserName && x.ReceiverUserName == request.ReceiverUserName);
            if (!messages.Any())
            {
                return new GetUserMessagesByUserNameQueryResult()
                {
                    Success = false,
                    Messages = new List<MessageDto>()
                    {
                        new MessageDto() {Message = $"{request.ReceiverUserName} aranızda mesajınız bulunmamaktadır"}
                    }
                };
            }

            return new GetUserMessagesByUserNameQueryResult()
            {
                Success = true,
                Result = messages.Select(x => new GetUserMessagesByUserNameDto()
                {
                    Message = x.Content,
                    CreatedDate = x.CreatedDate
                }).ToList()
            };
        }
    }
}