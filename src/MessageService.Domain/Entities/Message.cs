using MessageService.Domain.Constants;
using MessageService.Domain.Entities.Base;
using MessageService.Domain.Exceptions;

namespace MessageService.Domain.Entities
{
    public class Message : BaseEntity
    {
        public string Sender { get; private set; }
        public string SenderUserName { get; private set; }
        public string Receiver { get; private set; }
        public string ReceiverUserName { get; private set; }
        public string Content { get; private set; }
        public DateTime CreatedDate { get; private set; }

        private Message(string sender, string senderUserName, string receiver, string receiverUserName, string content, DateTime createdDate)
        {
            Sender = sender;
            SenderUserName = senderUserName;
            Receiver = receiver;
            ReceiverUserName = receiverUserName;
            Content = content;
            CreatedDate = createdDate;
        }

        public static Message Create(string sender, string senderUserName, string receiver, string receiverUserName, string content)
        {
            if (string.IsNullOrEmpty(sender))
                throw new DomainException(DomainErrorMessage.DomainError9);

            if (string.IsNullOrEmpty(senderUserName))
                throw new DomainException(DomainErrorMessage.DomainError12);

            if (string.IsNullOrEmpty(receiver))
                throw new DomainException(DomainErrorMessage.DomainError10);

            if (string.IsNullOrEmpty(receiverUserName))
                throw new DomainException(DomainErrorMessage.DomainError13);

            if (string.IsNullOrEmpty(content))
                throw new DomainException(DomainErrorMessage.DomainError11);

            return new Message(sender, senderUserName, receiver, receiverUserName, content, DateTime.Now);
        }
    }
}