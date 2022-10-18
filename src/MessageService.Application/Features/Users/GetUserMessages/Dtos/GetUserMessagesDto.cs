namespace MessageService.Application.Features.Users.GetUserMessages.Dtos
{
    public class GetUserMessagesDto
    {
        public string Content { get; set; }
        public string Receiver { get; set; }
        public string ReceiverUserName { get; set; }
    }
}