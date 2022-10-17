using FluentValidation;

namespace MessageService.Application.Features.Accounts.Login.Commands
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