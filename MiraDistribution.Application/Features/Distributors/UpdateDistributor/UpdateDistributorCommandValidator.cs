using FluentValidation;

namespace MiraDistribution.Application.Features.Distributors.UpdateDistributor
{
    public class UpdateDistributorCommandValidator : AbstractValidator<UpdateDistributorCommand>
    {
        public UpdateDistributorCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("اسم الموزع مطلوب.");
            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("رقم التليفون مطلوب.")
                .Matches(@"^01[0-9]{9}$").WithMessage("رقم التليفون لازم يكون رقم مصري صحيح.");
        }
    }
}
