using MessageService.Infrastructure.Dtos;

namespace MessageService.Application.Base
{
    public class BaseResult
    {
        public bool Success { get; set; }
        public List<MessageDto> Messages { get; set; }
    }
}