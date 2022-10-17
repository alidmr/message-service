namespace MessageService.Application.Events
{
    public class UserLogEvent
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