using MediatR;
using MessageService.Application.Constants;
using MessageService.Application.Features.Users.GetUserLogs.Dtos;
using MessageService.Application.Features.Users.GetUserLogs.Validator;
using MessageService.Domain.Repositories;
using MessageService.Infrastructure.Dtos;

namespace MessageService.Application.Features.Users.GetUserLogs.Queries
{
    public class GetUserLogsQueryHandler : IRequestHandler<GetUserLogsQuery, GetUserLogsQueryResult>
    {
        private readonly IUserLogRepository _userLogRepository;

        public GetUserLogsQueryHandler(IUserLogRepository userLogRepository)
        {
            _userLogRepository = userLogRepository;
        }

        public async Task<GetUserLogsQueryResult> Handle(GetUserLogsQuery request, CancellationToken cancellationToken)
        {
            var validation = await new GetUserLogsQueryValidator().ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
            {
                return new GetUserLogsQueryResult()
                {
                    Success = false,
                    Messages = validation.Errors.Select(x => new MessageDto()
                    {
                        Message = x.ErrorMessage
                    }).ToList()
                };
            }

            var userLogs = await _userLogRepository.GetAllAsync(x => x.UserName == request.UserName);
            if (!userLogs.Any())
            {
                return new GetUserLogsQueryResult()
                {
                    Success = false,
                    Messages = new List<MessageDto>() {new MessageDto() {Message = ApplicationErrorMessage.ApplicationError10}}
                };
            }

            return new GetUserLogsQueryResult()
            {
                Success = true,
                Result = userLogs.Select(x => new GetUserLogsDto()
                {
                    UserName = x.UserName,
                    Content = x.Content,
                    CreatedDate = x.CreatedDate
                }).ToList()
            };
        }
    }
}