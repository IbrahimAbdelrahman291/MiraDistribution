using FluentValidation;

namespace MiraDistribution.Application.Features.Books.CreateBook
{
    public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
    {
        public CreateBookCommandValidator()
        {
            RuleFor(x => x.SerialStart).GreaterThan(0).WithMessage("رقم بداية التسلسل لازم يكون أكبر من صفر.");
            RuleFor(x => x.Type).IsInEnum();
        }
    }
}
