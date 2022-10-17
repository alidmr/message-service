using MessageService.Domain.Constants;
using MessageService.Domain.Entities.Base;
using MessageService.Domain.Exceptions;

namespace MessageService.Domain.Entities
{
    public class UserLog : BaseEntity
    {
        public string UserName { get; private set; }
        public string Content { get; private set; }
        public DateTime CreatedDate { get; private set; }

        private UserLog(string userName, string content)
        {
            UserName = userName;
            Content = content;
            CreatedDate = DateTime.Now;
        }

        public static UserLog Create(string userName, string content)
        {
            if (string.IsNullOrEmpty(userName))
                throw new DomainException(DomainErrorMessage.DomainError7);
            if (string.IsNullOrEmpty(content))
                throw new DomainException(DomainErrorMessage.DomainError8);

            return new UserLog(userName, content);
        }
    }
}