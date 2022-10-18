using MediatR;
using MessageService.Application.Constants;
using MessageService.Application.Features.Users.BlockUsers.Validator;
using MessageService.Domain.Entities;
using MessageService.Domain.Repositories;
using MessageService.Infrastructure.Dtos;

namespace MessageService.Application.Features.Users.BlockUsers.Command
{
    public class BlockUserCommandHandler : IRequestHandler<BlockUserCommand, BlockUserCommandResult>
    {
        private readonly IBlockUserRepository _blockUserRepository;
        private readonly IUserRepository _userRepository;

        public BlockUserCommandHandler(IBlockUserRepository blockUserRepository, IUserRepository userRepository)
        {
            _blockUserRepository = blockUserRepository;
            _userRepository = userRepository;
        }

        public async Task<BlockUserCommandResult> Handle(BlockUserCommand request, CancellationToken cancellationToken)
        {
            var validation = await new BlockUserCommandValidator().ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
            {
                return new BlockUserCommandResult()
                {
                    Success = false,
                    Messages = validation.Errors.Select(x => new MessageDto()
                    {
                        Message = x.ErrorMessage
                    }).ToList()
                };
            }

            var checkUser = await _userRepository.GetAsync(x => x.UserName == request.BlockedUserName);
            if (checkUser == null)
            {
                return new BlockUserCommandResult()
                {
                    Success = false,
                    Messages = new List<MessageDto>() {new MessageDto() {Message = ApplicationErrorMessage.ApplicationError9}}
                };
            }

            var blockUser = BlockUser.Create(request.BlockingUserName, request.BlockedUserName);
            await _blockUserRepository.AddAsync(blockUser);

            return new BlockUserCommandResult()
            {
                Success = true,
                Messages = new List<MessageDto>() {new MessageDto() {Message = "İşlem başarılı"}}
            };
        }
    }
}