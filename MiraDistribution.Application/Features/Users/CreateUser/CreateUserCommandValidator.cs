using FluentValidation;
using MiraDistribution.Domain.Enums;

namespace MiraDistribution.Application.Features.Users.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("رقم التليفون مطلوب.")
                .Matches(@"^01[0-9]{9}$").WithMessage("رقم التليفون لازم يكون رقم مصري صحيح (01xxxxxxxxx).");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("كلمة المرور مطلوبة.")
                .MinimumLength(6).WithMessage("كلمة المرور لازم تكون 6 حروف/أرقام على الأقل.");

            RuleFor(x => x.Role)
                .Must(r => r == UserRole.Accountant || r == UserRole.Distributor)
                .WithMessage("تقدر بس تنشئ حساب محاسب أو موزع من هنا.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("اسم الموزع مطلوب.")
                .When(x => x.Role == UserRole.Distributor);
        }
    }
}
