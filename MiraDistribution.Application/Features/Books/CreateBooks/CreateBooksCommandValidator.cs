using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiraDistribution.Application.Features.Books.CreateBooks
{
    public class CreateBooksCommandValidator : AbstractValidator<CreateBooksCommand>
    {
        public CreateBooksCommandValidator()
        {
            RuleFor(x => x.FirstSerialStart)
                .GreaterThan(0).WithMessage("رقم بداية التسلسل لازم يكون أكبر من صفر.");

            RuleFor(x => x.Count)
                .GreaterThan(0).WithMessage("عدد الدفاتر لازم يكون أكبر من صفر.")
                .LessThanOrEqualTo(200).WithMessage("مينفعش تضيف أكتر من 200 دفتر في المرة الواحدة.");

            RuleFor(x => x.Type).IsInEnum();
        }
    }
}
