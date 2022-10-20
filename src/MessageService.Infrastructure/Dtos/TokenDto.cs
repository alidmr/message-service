namespace MessageService.Infrastructure.Dtos
{
    public class TokenDto
    {
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}