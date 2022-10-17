using FluentValidation;
using MessageService.Application.Constants;

namespace MessageService.Application.Features.Accounts.Commands
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty()
                .WithMessage(ApplicationErrorMessage.ApplicationError1);

            RuleFor(x => x.LastName).NotEmpty()
                .WithMessage(ApplicationErrorMessage.ApplicationError2);

            RuleFor(x => x.UserName).NotEmpty()
                .WithMessage(ApplicationErrorMessage.ApplicationError4);
            
            RuleFor(x => x.Password).NotEmpty()
                .WithMessage(ApplicationErrorMessage.ApplicationError5);
        }
    }
}