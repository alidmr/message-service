using System.Security.Claims;

namespace MessageService.Api.Auth
{
    public class ApplicationUser : IApplicationUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsAuthenticate
        {
            get => _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
            set => ((IApplicationUser) this).IsAuthenticate = value;
        }

        public string UserId
        {
            get => _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            set => ((IApplicationUser) this).UserId = value;
        }

        public string UserName
        {
            get => _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserName").Value;
            set => ((IApplicationUser) this).UserName = value;
        }

        public string FirstName
        {
            get => _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
            set => ((IApplicationUser) this).FirstName = value;
        }

        public string LastName
        {
            get => _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Surname).Value;
            set => ((IApplicationUser) this).LastName = value;
        }
    }
}