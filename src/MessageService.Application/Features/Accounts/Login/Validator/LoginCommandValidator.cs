using FluentValidation;
using MessageService.Application.Features.Accounts.Login.Commands;

namespace MessageService.Application.Features.Accounts.Login.Validator
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.UserName).NotEmpty()
                .WithMessage("Kullanıcı adı boş olamaz");

            RuleFor(x => x.Password).NotEmpty()
                .WithMessage("Şifre adresi boş olamaz");
        }
    }
}