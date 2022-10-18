using MessageService.Application.Base;
using MessageService.Application.Features.Users.GetUserLogs.Dtos;

namespace MessageService.Application.Features.Users.GetUserLogs.Queries
{
    public class GetUserLogsQueryResult : BaseResult
    {
        public List<GetUserLogsDto> Result { get; set; }
    }
}