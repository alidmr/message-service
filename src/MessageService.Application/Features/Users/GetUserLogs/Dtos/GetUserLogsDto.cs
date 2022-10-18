namespace MessageService.Application.Features.Users.GetUserLogs.Dtos
{
    public class GetUserLogsDto
    {
        public string UserName { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}