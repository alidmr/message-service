using MessageService.Domain.Constants;
using MessageService.Domain.Entities.Base;
using MessageService.Domain.Exceptions;

namespace MessageService.Domain.Entities
{
    public class Message : BaseEntity
    {
        public string Sender { get; private set; }
        public string Receiver { get; private set; }
        public string Content { get; private set; }
        public DateTime CreatedDate { get; private set; }

        private Message(string sender, string receiver, string content)
        {
            Sender = sender;
            Receiver = receiver;
            Content = content;
            CreatedDate = DateTime.Now;
        }

        public static Message Create(string sender, string receiver, string content)
        {
            if (string.IsNullOrEmpty(sender))
                throw new DomainException(DomainErrorMessage.DomainError9);
            if (string.IsNullOrEmpty(receiver))
                throw new DomainException(DomainErrorMessage.DomainError10);
            if (string.IsNullOrEmpty(content))
                throw new DomainException(DomainErrorMessage.DomainError11);

            return new Message(sender, receiver, content);
        }
    }
}