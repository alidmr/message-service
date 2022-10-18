using FluentValidation;
using MessageService.Application.Features.Users.GetUserLogs.Queries;

namespace MessageService.Application.Features.Users.GetUserLogs.Validator
{
    public class GetUserLogsQueryValidator : AbstractValidator<GetUserLogsQuery>
    {
        public GetUserLogsQueryValidator()
        {
            RuleFor(x => x.UserName).NotEmpty()
                .WithMessage("Kullanı adı boş olamaz");
        }
    }
}