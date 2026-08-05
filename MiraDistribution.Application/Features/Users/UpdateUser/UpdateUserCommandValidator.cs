using FluentValidation;

namespace MiraDistribution.Application.Features.Users.UpdateUser
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.Phone)
                .Matches(@"^01[0-9]{9}$").WithMessage("رقم التليفون لازم يكون رقم مصري صحيح.")
                .When(x => !string.IsNullOrEmpty(x.Phone));

            RuleFor(x => x.Password)
                .MinimumLength(6).WithMessage("كلمة المرور لازم تكون 6 حروف/أرقام على الأقل.")
                .When(x => !string.IsNullOrEmpty(x.Password));
        }
    }
}