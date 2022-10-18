using FluentValidation;
using MessageService.Application.Features.Users.BlockUsers.Command;

namespace MessageService.Application.Features.Users.BlockUsers.Validator
{
    public class BlockUserCommandValidator : AbstractValidator<BlockUserCommand>
    {
        public BlockUserCommandValidator()
        {
            RuleFor(x => x.BlockingUserName).NotEmpty().WithMessage("Engelleyen kullanıcı boş olamaz");
            RuleFor(x => x.BlockedUserName).NotEmpty().WithMessage("Engellenen kullanıcı boş olamaz");
        }
    }
}