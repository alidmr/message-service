using MediatR;

namespace MessageService.Application.Events.Users
{
    public class UserLogEvent : IEvent, INotification
    {
        public string UserName { get; private set; }
        public string Content { get; private set; }

        public UserLogEvent(string userName, string content)
        {
            UserName = userName;
            Content = content;
        }
    }
}