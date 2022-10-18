using MediatR;
using MessageService.Application.Constants;
using MessageService.Application.Features.Users.GetUsers.Dtos;
using MessageService.Domain.Repositories;
using MessageService.Infrastructure.Dtos;

namespace MessageService.Application.Features.Users.GetUsers.Queries
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, GetUsersQueryResult>
    {
        private readonly IUserRepository _userRepository;

        public GetUsersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<GetUsersQueryResult> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllAsync();
            if (!users.Any())
            {
                return new GetUsersQueryResult()
                {
                    Success = false,
                    Messages = new List<MessageDto>() {new MessageDto() {Message = ApplicationErrorMessage.ApplicationError12}}
                };
            }

            return new GetUsersQueryResult()
            {
                Success = true,
                Result = users.Select(x => new UserDto()
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    UserName = x.UserName
                }).ToList()
            };
        }
    }
}