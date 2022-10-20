using MediatR;
using MessageService.Application.Events.Users;
using MessageService.Domain.Repositories;

namespace MessageService.Application.EventHandlers.UserLogs
{
    public class UserLogEventHandler : INotificationHandler<UserLogEvent> //IRequestHandler<UserLogEvent, bool>
    {
        private readonly IUserLogRepository _userLogRepository;

        public UserLogEventHandler(IUserLogRepository userLogRepository)
        {
            _userLogRepository = userLogRepository;
        }

        public async Task Handle(UserLogEvent notification, CancellationToken cancellationToken)
        {
            var userLog = Domain.Entities.UserLog.Create(notification.UserName, notification.Content);
            await _userLogRepository.AddAsync(userLog);
        }
    }
}