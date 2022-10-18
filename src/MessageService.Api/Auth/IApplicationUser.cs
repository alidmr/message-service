namespace MessageService.Api.Auth
{
    public interface IApplicationUser
    {
        bool IsAuthenticate { get; set; }
        string UserId { get; set; }
        string UserName { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
    }
}