namespace MessageService.Application.Events.Message
{
    public class MessageCreatedEvent
    {
        public MessageCreatedEvent(string sender, string receiver, string content, string senderUserName, string receiverUserName)
        {
            Sender = sender;
            Receiver = receiver;
            Content = content;
            SenderUserName = senderUserName;
            ReceiverUserName = receiverUserName;
        }

        public string Sender { get; private set; }
        public string SenderUserName { get; private set; }
        public string Receiver { get; private set; }
        public string ReceiverUserName { get; private set; }
        public string Content { get; private set; }
    }
}