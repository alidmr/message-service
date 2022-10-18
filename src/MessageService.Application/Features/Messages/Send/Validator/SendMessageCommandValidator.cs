using FluentValidation;
using MessageService.Application.Features.Messages.Send.Commands;

namespace MessageService.Application.Features.Messages.Send.Validator
{
    public class SendMessageCommandValidator : AbstractValidator<SendMessageCommand>
    {
        public SendMessageCommandValidator()
        {
            RuleFor(x => x.Sender).NotEmpty()
                .WithMessage("Gönderen boş olamaz");
            RuleFor(x => x.Receiver).NotEmpty()
                .WithMessage("Alıcı boş olamaz");
            RuleFor(x => x.ReceiverUserName).NotEmpty()
                .WithMessage("Alıcı kullanıcı adı boş olamaz");
            RuleFor(x => x.MessageContent).NotEmpty()
                .WithMessage("Mesaj içeriği adı boş olamaz");
        }
    }
}