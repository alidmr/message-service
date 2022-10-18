using MessageService.Domain.Constants;
using MessageService.Domain.Entities.Base;
using MessageService.Domain.Exceptions;

namespace MessageService.Domain.Entities
{
    public class BlockUser : BaseEntity
    {
        public string Blocking { get; private set; }
        public string Blocked { get; private set; }
        public DateTime CreatedDate { get; private set; }

        public BlockUser(string blocking, string blocked, DateTime createdDate)
        {
            Blocking = blocking;
            Blocked = blocked;
            CreatedDate = createdDate;
        }

        public static BlockUser Create(string blocking, string blocked)
        {
            if (string.IsNullOrEmpty(blocking))
                throw new DomainException(DomainErrorMessage.DomainError14);
            if (string.IsNullOrEmpty(blocked))
                throw new DomainException(DomainErrorMessage.DomainError15);

            return new BlockUser(blocking, blocked, DateTime.Now);
        }
    }
}