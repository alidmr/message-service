using MediatR;

namespace MessageService.Application.Features.Users.GetUserLogs.Queries
{
    public class GetUserLogsQuery : IRequest<GetUserLogsQueryResult>
    {
        public string UserName { get; set; }
    }
}