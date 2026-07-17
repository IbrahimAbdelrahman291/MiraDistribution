using FluentValidation;

namespace MiraDistribution.Application.Features.Books.UpdateBookSerial
{
    public class UpdateBookSerialCommandValidator : AbstractValidator<UpdateBookSerialCommand>
    {
        public UpdateBookSerialCommandValidator()
        {
            RuleFor(x => x.NewSerialStart).GreaterThan(0).WithMessage("رقم بداية التسلسل لازم يكون أكبر من صفر.");
        }
    }

}
