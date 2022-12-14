using MessageService.Domain.Constants;
using MessageService.Domain.Entities.Base;
using MessageService.Domain.Exceptions;

namespace MessageService.Domain.Entities
{
    public class User : BaseEntity
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }
        public string PasswordSalt { get; private set; }
        public DateTime CreatedDate { get; private set; }

        private User(string firstName, string lastName, string userName, DateTime createdDate)
        {
            FirstName = firstName;
            LastName = lastName;
            UserName = userName;
            CreatedDate = createdDate;
        }

        public static User Create(string firstName, string lastName, string userName)
        {
            if (string.IsNullOrEmpty(firstName))
                throw new DomainException(DomainErrorMessage.DomainError1);

            if (string.IsNullOrEmpty(lastName))
                throw new DomainException(DomainErrorMessage.DomainError2);

            if (string.IsNullOrEmpty(userName))
                throw new DomainException(DomainErrorMessage.DomainError4);

            return new User(firstName, lastName, userName, DateTime.Now);
        }

        public User SetPassword(string password, string passwordSalt)
        {
            if (string.IsNullOrEmpty(password))
                throw new DomainException(DomainErrorMessage.DomainError5);

            if (string.IsNullOrEmpty(passwordSalt))
                throw new DomainException(DomainErrorMessage.DomainError6);

            Password = password;
            PasswordSalt = passwordSalt;
            return this;
        }
    }
}