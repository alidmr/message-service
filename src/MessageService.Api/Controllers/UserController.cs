using MediatR;
using MessageService.Api.Auth;
using MessageService.Application.Features.Users.BlockUsers.Command;
using MessageService.Application.Features.Users.BlockUsers.Queries;
using MessageService.Application.Features.Users.GetUserLogs.Queries;
using MessageService.Application.Features.Users.GetUserMessages.Queries;
using MessageService.Application.Features.Users.GetUserMessagesByUserName.Queries;
using MessageService.Application.Features.Users.GetUsers.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessageService.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IApplicationUser _applicationUser;

        public UserController(IMediator mediator, IApplicationUser applicationUser)
        {
            _mediator = mediator;
            _applicationUser = applicationUser;
        }

        [HttpGet]
        public async Task<GetUsersQueryResult> GetUsers()
        {
            var result = await _mediator.Send(new GetUsersQuery());
            return result;
        }

        [HttpGet]
        [Route("logs")]
        public async Task<GetUserLogsQueryResult> GetUserLogs()
        {
            var currentUserName = _applicationUser.UserName;
            var result = await _mediator.Send(new GetUserLogsQuery()
            {
                UserName = currentUserName
            });
            return result;
        }

        [HttpGet]
        [Route("messages")]
        public async Task<GetUserMessagesQueryResult> GetMessages()
        {
            var result = await _mediator.Send(new GetUserMessagesQuery()
            {
                UserName = _applicationUser.UserName
            });
            return result;
        }

        [HttpGet]
        [Route("{userName}/messages")]
        public async Task<GetUserMessagesByUserNameQueryResult> GetMessagesByUserName(string userName)
        {
            var result = await _mediator.Send(new GetUserMessagesByUserNameQuery
            {
                ReceiverUserName = userName,
                SenderUserName = _applicationUser.UserName
            });
            return result;
        }

        [HttpPost]
        [Route("block-user")]
        public async Task<BlockUserCommandResult> BlockUser([FromBody] BlockUserCommand command)
        {
            command.BlockingUserName = _applicationUser.UserName;
            var result = await _mediator.Send(command);
            return result;
        }

        [HttpGet]
        [Route("blocked-users")]
        public async Task<GetBlockedUsersQueryResult> GetBlockedUsers()
        {
            var result = await _mediator.Send(new GetBlockedUsersQuery()
            {
                BlockingUserName = _applicationUser.UserName
            });
            return result;
        }
    }
}