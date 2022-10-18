using FluentValidation;
using MessageService.Application.Features.Users.GetUserMessages.Queries;

namespace MessageService.Application.Features.Users.GetUserMessages.Validator
{
    public class GetUserMessagesQueryValidator : AbstractValidator<GetUserMessagesQuery>
    {
        public GetUserMessagesQueryValidator()
        {
            RuleFor(x => x.UserName).NotEmpty()
                .WithMessage("Kullanıcı adı boş geçilemez");
        }
    }
}