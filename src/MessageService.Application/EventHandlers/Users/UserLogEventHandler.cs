using MediatR;
using MessageService.Application.Events.Users;
using MessageService.Domain.Repositories;

namespace MessageService.Application.EventHandlers.Users
{
    public class UserLogEventHandler : INotificationHandler<UserLogEvent>
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

            await Task.CompletedTask;
        }
    }
}