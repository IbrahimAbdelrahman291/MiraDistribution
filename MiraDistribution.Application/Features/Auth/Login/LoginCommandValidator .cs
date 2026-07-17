using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiraDistribution.Application.Features.Auth.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Phone).NotEmpty().WithMessage("رقم التليفون مطلوب.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("كلمة المرور مطلوبة.");
        }
    }
}
