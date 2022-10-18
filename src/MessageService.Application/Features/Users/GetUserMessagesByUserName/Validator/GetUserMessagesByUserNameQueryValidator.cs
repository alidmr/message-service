using FluentValidation;
using MessageService.Application.Features.Users.GetUserMessagesByUserName.Queries;

namespace MessageService.Application.Features.Users.GetUserMessagesByUserName.Validator
{
    public class GetUserMessagesByUserNameQueryValidator : AbstractValidator<GetUserMessagesByUserNameQuery>
    {
        public GetUserMessagesByUserNameQueryValidator()
        {
            RuleFor(x => x.SenderUserName).NotEmpty()
                .WithMessage("Gönderen kullanıcı adı boş olamaz");
            RuleFor(x => x.ReceiverUserName).NotEmpty()
                .WithMessage("Alıcı kullanıcı adı boş olamaz");
        }
    }
}